using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using System.IO;
using System.Windows.Forms;
using Rhino;
using Rhino.Geometry;
using Rhino.FileIO;
using Rhino.DocObjects;
using System.Net.Mail;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using static ReadRhinoFile.Helpers.EmailSender;
using static ReadRhinoFile.UserForm;
using System.Diagnostics;

namespace ReadRhinoFile
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class TransferTool : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Autodesk.Revit.DB.Document doc = uidoc.Document;

            // Open file dialog to select a Rhino .3dm file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Rhino Files (*.3dm)|*.3dm";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                // Output lists
                List<Rhino.Geometry.Plane> planes = new List<Rhino.Geometry.Plane>();
                List<Rhino.Geometry.Point3d> locations = new List<Rhino.Geometry.Point3d>();
                List<string> blockNames = new List<string>();
                List<double> scales = new List<double>();
                string modelUnitSystem;

                int selectedCPlaneIndex;
                int selectedRevitOrigin = 1;
                string selectedRevitWorkset;

                XYZ revitBasePoint;
                XYZ revitSurveyPoint;
                XYZ internalPoint;
                XYZ newRevitInternalPoint;
                bool worksharing = doc.IsWorkshared;
                List<string> worksets = new List<string>();

                    FilteredWorksetCollector worksetCollector = new FilteredWorksetCollector(doc);
                    FilteredWorksetCollector userWorksets = worksetCollector.OfKind(WorksetKind.UserWorkset);
                    foreach (var workset in userWorksets)
                    {
                        worksets.Add(workset.Name);
                    }

                // Load Rhino file
                using (Rhino.FileIO.File3dm file = Rhino.FileIO.File3dm.Read(filePath))
                {
                    // Check if the file is successfully loaded
                    if (file == null)
                    {
                        return Result.Cancelled;
                    }

                    // Get the model unit system
                    File3dmSettings settings = file.Settings;
                    modelUnitSystem = settings.ModelUnitSystem.ToString();

                    List<string> cplanesNames = new List<string> { "WorldXY" };
                    List<Rhino.Geometry.Plane> cplanes = new List<Rhino.Geometry.Plane> { Rhino.Geometry.Plane.WorldXY };
                    Vector3d cplaneMapping = new Vector3d(0,0,0);

                    foreach (ConstructionPlane cplane in file.NamedConstructionPlanes)
                    {
                        cplanesNames.Add(cplane.Name);
                        cplanes.Add(cplane.Plane);
                    }

                        ReadRhinoFile.UserForm form = new ReadRhinoFile.UserForm(cplanesNames, worksets, worksharing);
                        DialogResult result = form.ShowDialog();
                        selectedCPlaneIndex = form.SelectedCPlaneIndex;
                        selectedRevitOrigin = form.SelectedRevitOrigin;
                        selectedRevitWorkset = form.SelectedRevitWorkset;

                        //BasePoint and SurveyPoint
                        BasePoint projectBasePoint = BasePoint.GetProjectBasePoint(doc);
                        BasePoint surveyPoint = BasePoint.GetSurveyPoint(doc);

                        revitBasePoint = projectBasePoint.Position;
                        revitSurveyPoint = surveyPoint.Position;
                        internalPoint = new XYZ(0, 0, 0);
                        newRevitInternalPoint = new XYZ(0, 0, 0);

                        newRevitInternalPoint =  (selectedRevitOrigin == 1) ? projectBasePoint.Position :
                                                 (selectedRevitOrigin == 2) ? surveyPoint.Position : newRevitInternalPoint;

                    if (result == DialogResult.Cancel)
                    {
                        return Result.Cancelled;
                    }

                    foreach (var obj in file.Objects)
                    {

                        // Check if the object is an instance definition reference
                        if (obj.Geometry.ObjectType == Rhino.DocObjects.ObjectType.InstanceReference)
                        {
                            InstanceReferenceGeometry instanceRefGeometry = obj.Geometry as InstanceReferenceGeometry;

                            // Ensure it's a valid instance reference geometry
                            if (instanceRefGeometry != null)
                            {
                                // Get the instance definition ID and name
                                var idefId = instanceRefGeometry.ParentIdefId;
                                var idef = file.AllInstanceDefinitions.FindId(idefId);
                                var name = idef.Name;
                                string newName = name.Replace(":", "");
                                blockNames.Add(newName);

                                Rhino.Geometry.Line line = new Rhino.Geometry.Line(cplanes[selectedCPlaneIndex].Origin, Point3d.Origin);
                                //Vector of the mapping
                                cplaneMapping = line.Direction;

                                double angleInRadians;

                                // Calculate the cosine of the angle between the normal vectors and the Y-axis or X-axis
                                Vector3d vector1 = Vector3d.YAxis;
                                Vector3d vector2 = cplanes[selectedCPlaneIndex].YAxis;

                                if (cplanes[selectedCPlaneIndex].ZAxis.Z > 0)
                                {
                                    angleInRadians = Vector3d.VectorAngle(vector1, vector2);
                                }
                                else
                                {
                                    vector1 = Vector3d.XAxis;
                                    angleInRadians = Vector3d.VectorAngle(vector1, vector2);
                                }

                                // Check the sign of the z-component of the cross product to determine the direction
                                Vector3d crossProduct = Vector3d.CrossProduct(vector2, vector1);

                                if (crossProduct.Z < 0)
                                {
                                    // Adjust the angle for counterclockwise rotation (left direction)
                                    angleInRadians = 2 * Math.PI - angleInRadians;
                                }

                                // Convert radians to degrees
                                double angleInDegrees = RhinoMath.ToDegrees(angleInRadians);

                                // Get the transformation matrix
                                Rhino.Geometry.Transform transform = instanceRefGeometry.Xform;

                                // Extract translation, X-axis, Y-axis vectors from the transformation matrix
                                //Add mapping from CPlane to Rhino if CPlane is specified
                                Point3d location = new Point3d(transform.M03, transform.M13, transform.M23) + cplaneMapping;
                                Vector3d vectorX = new Vector3d(transform.M00, transform.M10, transform.M20);
                                Vector3d vectorY = new Vector3d(transform.M01, transform.M11, transform.M21);

                                // Create a plane from the extracted information
                                Rhino.Geometry.Plane plane = new Rhino.Geometry.Plane(location, vectorX, vectorY);

                                // Create a rotation transformation
                                Rhino.Geometry.Transform rotationTransform = Rhino.Geometry.Transform.Rotation(angleInRadians, Vector3d.ZAxis, Point3d.Origin);

                                plane.Transform(rotationTransform);

                                // Extract the scaling factor from the transformation matrix
                                Vector3d scaleVector = new Vector3d(transform.M02, transform.M12, transform.M22);
                                double scale = scaleVector.Length;

                                //adjust scale if units were changed - how to do it in better way?
                                if (scale == 0.001) scale = 1;
                                if (scale == 0.1) scale = 1;

                                // Add data to respective lists
                                locations.Add(plane.Origin);
                                planes.Add(plane);
                                scales.Add(scale);
                            }
                        }
                    }

                }

                var time = Stopwatch.StartNew();

                string hostPath = @"\Revit\Enscape AssetDefinitions 2023\";
                int instanceNum = 0;

                string[] fileNames = File.ReadAllLines(@"\Revit\AssetsNames.csv");
                HashSet<string> fileNamesHashSet = new HashSet<string>(fileNames);

                time.Stop();
                double timeRun = time.ElapsedMilliseconds;
                int test =  0;

                foreach (string fileInfo in fileNames)
                {
                    fileNamesHashSet.Add(fileInfo);
                }

                //Units 
                ForgeTypeId unit;
                switch (modelUnitSystem)
                {
                    case "Millimeters":
                        unit = UnitTypeId.Millimeters;
                        break;
                    case "Centimeters":
                        unit = UnitTypeId.Centimeters;
                        break;
                    case "Decimeters":
                        unit = UnitTypeId.Decimeters;
                        break;
                    case "Meters":
                        unit = UnitTypeId.Meters;
                        break;

                    default:
                        unit = UnitTypeId.Millimeters;
                        break;
                }

                double feetToUnit = UnitUtils.ConvertFromInternalUnits(1, unit);

                try
                {
                    for (int i = 0; i < blockNames.Count; i++)
                    {
                        string familyName = "Enscape AssetDefinition - " + blockNames[i];
                        Family family;

                        if (!fileNamesHashSet.Contains(familyName))
                        {
                            int lastSpaceIndex = familyName.LastIndexOf(' ');
                            string modifiedFileName = lastSpaceIndex >= 0 ? familyName.Substring(0, lastSpaceIndex) : familyName;

                            if (fileNamesHashSet.Contains(modifiedFileName))
                            {
                                familyName = modifiedFileName;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        try
                        {
                            //Always load the basic family with original height value
                            Autodesk.Revit.DB.Document docHost = app.OpenDocumentFile(hostPath + familyName + ".rfa");
                            family = docHost.LoadFamily(doc);
                        }

                        catch
                        {
                            continue;
                        }

                        //If instance def is scaled
                        if (Math.Round(scales[i], 3) != 1.000)
                        {
                            using (Transaction transactionDuplicate = new Transaction(doc, "Duplicate Family Type"))
                            {
                                transactionDuplicate.Start();

                                // Get the family symbol ids in the family
                                ISet<ElementId> symbolIds = family.GetFamilySymbolIds();

                                // Assume you want to duplicate the first family type in the family
                                FamilySymbol familySymbolToDuplicate = doc.GetElement(symbolIds.First()) as FamilySymbol;

                                familyName = "Enscape AssetDefinition - " + blockNames[i] + " - Scaled_" + Math.Round(scales[i], 2);

                                // Duplicate the family type
                                try
                                {
                                    FamilySymbol duplicatedFamilySymbol = familySymbolToDuplicate.Duplicate(familyName) as FamilySymbol;

                                    var param = duplicatedFamilySymbol.GetParameters("Height");

                                    if (param[0] != null)
                                    {
                                        double value = param[0].AsDouble();
                                        param[0].Set(value * scales[i]); //add here scales[i];
                                    }
                                }

                                catch
                                {
                                }

                                transactionDuplicate.Commit();
                            }
                        }             

                        double xValue = locations[i].X / feetToUnit;
                        double yValue = locations[i].Y / feetToUnit;
                        double zValue = locations[i].Z / feetToUnit;

                        XYZ movementVector = newRevitInternalPoint - internalPoint;
                        XYZ blockOrigin = new XYZ(xValue, yValue, zValue) + movementVector;

                        Autodesk.Revit.DB.Plane plane = Autodesk.Revit.DB.Plane.CreateByOriginAndBasis(new XYZ(planes[i].OriginX, planes[i].OriginY, planes[i].OriginZ),
                        new XYZ(planes[i].XAxis.X, planes[i].XAxis.Y, planes[i].XAxis.Z),
                        new XYZ(planes[i].YAxis.X, planes[i].YAxis.Y, planes[i].YAxis.Z));

                        // Get the FamilySymbol of the loaded family
                        FilteredElementCollector collector = new FilteredElementCollector(doc);
                        ICollection<ElementId> familySymbolIds = collector.OfClass(typeof(FamilySymbol)).ToElementIds();

                        FamilySymbol familySymbol = null;
                        ElementType elementType = null;
                        foreach (ElementId id in familySymbolIds)
                        {
                            FamilySymbol symbol = doc.GetElement(id) as FamilySymbol;
                            ElementType elementT = doc.GetElement(id) as ElementType;

                            if (symbol != null && symbol.Name == familyName)
                            {
                                familySymbol = symbol;
                                elementType = elementT;
                                continue;
                            }
                        }

                        //Place an instance
                        if (familySymbol != null)
                        {
                            // Place an instance of the family in the active document
                            using (Transaction transaction = new Transaction(doc, "Place Family Instance"))
                            {
                                transaction.Start();

                                if (!familySymbol.IsActive)
                                {
                                    familySymbol.Activate();
                                }

                                SketchPlane sketchPlane = SketchPlane.Create(doc, plane);
                                FamilyInstance newInstance = doc.Create.NewFamilyInstance(blockOrigin, familySymbol, sketchPlane, StructuralType.NonStructural);
                                instanceNum++;

                                //Worksets
                                if (worksharing)
                                {
                                    String targetWorksetName = selectedRevitWorkset;
                                    //Find target workset
                                    worksetCollector.OfKind(WorksetKind.UserWorkset);
                                    Workset workset = worksetCollector.FirstOrDefault<Workset>(ws => ws.Name == targetWorksetName);
                                    // Get the parameter that stores the workset id
                                    Parameter p = newInstance.GetParameter(ParameterTypeId.ElemPartitionParam);
                                    //newInstance.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM).Set(workset.Id.IntegerValue);
                                    p.Set(workset.Id.IntegerValue);
                                }

                                transaction.Commit();
                            }
                        }
                    }

                    string computerName = System.Environment.UserName;
                    //SendEmail(computerName, instanceNum);

                    return Result.Succeeded;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return Result.Failed;
                }
            }

            else
            {
                return Result.Cancelled;
            }

        }
    }
}
