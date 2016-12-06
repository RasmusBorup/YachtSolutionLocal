using System;
using System.Windows.Forms;

namespace Yachtsolution.ControlLayer
{
    class ImageController
    {
        private static ImageController instance;
        public static ImageController GetInstance()
        {
            if (instance == null)
            {
                instance = new ImageController();
            }
            return instance;
        }

        /// <summary>
        /// This method returns a path where the image lies on the computer.
        /// </summary>
        /// <returns>imgLoc</returns>
        public string BrowseImage()
        {
            string imgLoc = "";

            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = @"JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif|All Files (*.*)|*.*";
                dlg.Title = "Select Image";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    imgLoc = dlg.FileName;
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine("Couldn't browse the image.");
                Console.WriteLine("Error: " + exception.Message);
            }

            return imgLoc;
        }
    }
}
