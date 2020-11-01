using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CPATCMSApp.Models.Common.File
{
    public class InputFile
    {

        private readonly IHostingEnvironment hostingEnvironment;

        public InputFile(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }


        public bool Uploadfile(string filePath, IFormFile file,string uniqueFileName)
        {
            
            string destinationFolderName = Path.Combine(hostingEnvironment.WebRootPath, filePath);
            string finalFilePath = Path.Combine(destinationFolderName, uniqueFileName);

            FileStream tempFile = new FileStream(finalFilePath, FileMode.Create);
            file.CopyTo(tempFile);
            tempFile.Close();

            return true;
            //file.CopyTo(new FileStream(defectListFilePath, FileMode.Create));
        }

        public bool Updatefile(string filePath, IFormFile file, string oldFileName,string uniqueFileName)
        {
           
            string destinationFolderName = Path.Combine(hostingEnvironment.WebRootPath, filePath);
            string deletFilePath = Path.Combine(destinationFolderName, oldFileName);
            if (System.IO.File.Exists(deletFilePath))
            {
                System.IO.File.Delete(deletFilePath);

                string finalFilePath = Path.Combine(destinationFolderName, uniqueFileName);
                FileStream tempFile = new FileStream(finalFilePath, FileMode.Create);
                file.CopyTo(tempFile);
                tempFile.Close();
                // file.CopyTo(new FileStream(defectListFilePath, FileMode.Create));
                return true;
            }


            if (oldFileName=="No File")
            {
                
                string finalFilePath = Path.Combine(destinationFolderName, uniqueFileName);
                FileStream tempFile = new FileStream(finalFilePath, FileMode.Create);
                file.CopyTo(tempFile);
                tempFile.Close();
                // file.CopyTo(new FileStream(defectListFilePath, FileMode.Create));
                return true;
            }
            else
            {
                return false;
            }


        }


        public bool DeleteFile(string filePath, string fileName)
        {
            if (fileName != null)
            {
                string destinationFolderName = Path.Combine(hostingEnvironment.WebRootPath, filePath);
                string finalFilePath = Path.Combine(destinationFolderName, fileName);

                System.IO.File.Delete(finalFilePath);

                return true;

            }
            else
            {
                return false;
            }

            //file.CopyTo(new FileStream(defectListFilePath, FileMode.Create));
        }

    }
}
