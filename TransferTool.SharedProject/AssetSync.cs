using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.Policy;
using System.Windows.Media.Imaging;
using System.Windows.Forms;

namespace ReadRhinoFile
{
    internal class AssetSync : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "Henning Larsen";

            try
            {
                //application.GetRibbonPanels(tabName);
                application.CreateRibbonTab(tabName);
            }
            catch
            {
                //application.CreateRibbonTab(tabName);
            }

            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData("Import Assets2", "Import Assets2", assemblyPath, "EnscapeTransferTool.ReadRhinoFile.TransferTool");

            string panelName = "Enscape2";
            bool panelExist = false;
            RibbonPanel ribbonPanel = null;

            foreach (RibbonPanel panel in application.GetRibbonPanels(tabName))
            {
                if (panel.Name == panelName)
                {
                    ribbonPanel = panel;
                    panelExist = true;
                    break;
                }
            }

            if (!panelExist)
            {
                ribbonPanel = application.CreateRibbonPanel(tabName, panelName);
            }

            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

            pushButton.ToolTip = "Import your Enscape assets from Rhino to Revit";

            //Bitmap incon
            Uri urlImage = new Uri(@"H:\Denmark\Copenhagen\Sustainability\Engineering\Software tools\Revit\Icons\AssetSyncIcon32px.png");
            BitmapImage bitmapImage = new BitmapImage(urlImage);
            pushButton.LargeImage= bitmapImage;

            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

    }
}
