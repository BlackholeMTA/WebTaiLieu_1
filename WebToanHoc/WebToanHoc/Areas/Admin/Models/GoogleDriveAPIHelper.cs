using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace WebToanHoc.Areas.Admin.Models
{
     public class GoogleDriveAPIHelper
     {
          //add scope
          public static string[] Scopes = { Google.Apis.Drive.v3.DriveService.Scope.Drive };
          public static List<string> linkDrive = new List<string>();
        public static List<string> fileName = new List<string>();
        //modify
        public static string clientID = "code_secret_client_886569423884-ntd0v8qirbjfe48637pifuv2v6gc3dac.apps.googleusercontent.com.json";
        public static string yourProject = "Upload Drive";
        //create Drive API service.
        public static DriveService GetService()
          {
               //get Credentials from client_secret.json file 
               UserCredential credential;
               //Root Folder of project
               var CSPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");
               using (var stream = new FileStream(Path.Combine(CSPath, clientID), FileMode.Open, FileAccess.Read))
               {
                    String FolderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/"); ;
                    String FilePath = Path.Combine(FolderPath, "DriveServiceCredentials.json");
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(FilePath, true)).Result;
               }
               //create Drive API service.
               DriveService service = new Google.Apis.Drive.v3.DriveService(new BaseClientService.Initializer()
               {
                    HttpClientInitializer = credential,
                    ApplicationName = yourProject,
               });
               return service;
          }


          //file Upload to the Google Drive root folder.
          public static void UplaodFileOnDrive(HttpPostedFileBase file)
          {
               if (file != null && file.ContentLength > 0)
               {
                    //create service
                    DriveService service = GetService();
                    string path = Path.Combine(HttpContext.Current.Server.MapPath("~/GoogleDriveFiles"),
                    Path.GetFileName(file.FileName));
                
                file.SaveAs(path);
                var FileMetaData = new Google.Apis.Drive.v3.Data.File();
                    FileMetaData.Name = Path.GetFileName(file.FileName);
                    FileMetaData.MimeType = MimeMapping.GetMimeMapping(path);
                    FilesResource.CreateMediaUpload request;
                    using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
                    {
                         request = service.Files.Create(FileMetaData, stream, FileMetaData.MimeType);
                         request.Fields = "id";
                         request.Upload();
                    }
                    linkDrive.Add(GetLinkFile(file.FileName));
                //fileName.Add(FileMetaData.Name);
                //fileName.Add(Path.GetFileName(file.FileName));
               }
          }
          //get link file
          public static string GetLinkFile(string filename)
          {
               string linkdrive = "";
               Google.Apis.Drive.v3.DriveService service = GetService();
               // Define parameters of request.
               Google.Apis.Drive.v3.FilesResource.ListRequest request = service.Files.List();
               request.Q = "name='" + filename + "'";
               var result = request.Execute().Files;
               if (result != null)
               {
                    foreach (var item in result)
                    {
                         linkdrive += "https://drive.google.com/file/d/" + item.Id ;
                    break;
                    }
               }
               return linkdrive;
          }
          //get link folder
          //create folder on drive
          public static string GetLinkFolder(string folderName)
          {
               string linkdrive = "";
               Google.Apis.Drive.v3.DriveService service = GetService();
               // Define parameters of request.
               FilesResource.ListRequest request = service.Files.List();
               request.Q = "mimeType='application/vnd.google-apps.folder' and name='"+folderName+"'";
               //request.Fields = "files(id, name)";
               Google.Apis.Drive.v3.Data.FileList result = request.Execute();
               foreach (var item in result.Files)
               {
                    linkdrive = item.Id;
                    break;
               }
               
               return linkdrive;
          }
          public static void CreateFolder(string FolderName)
          {
               DriveService service = GetService();
               var FileMetaData = new Google.Apis.Drive.v3.Data.File();
               FileMetaData.Name = FolderName;
               //this mimetype specify that we need folder in google drive
               FileMetaData.MimeType = "application/vnd.google-apps.folder";
               FilesResource.CreateRequest request;
               request = service.Files.Create(FileMetaData);
               request.Fields = "id";
               var file = request.Execute();

          }
          // File upload in existing folder
          public static void FileUploadInFolder(string folderId, HttpPostedFileBase file)
          {
               if (file != null && file.ContentLength > 0)
               {
                    //create service
                    DriveService service = GetService();
                    //get file path
                    string path = Path.Combine(HttpContext.Current.Server.MapPath("~/GoogleDriveFiles"),
                    Path.GetFileName(file.FileName));
                file.SaveAs(path);
                //create file metadata
                var FileMetaData = new Google.Apis.Drive.v3.Data.File()
                    {
                         Name = Path.GetFileName(file.FileName),
                         MimeType = MimeMapping.GetMimeMapping(path),
                         //id of parent folder 
                         Parents = new List<string>
                    {
                        folderId
                    }
                    };
                    FilesResource.CreateMediaUpload request;
                    //create stream and upload
                    using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
                    {
                         request = service.Files.Create(FileMetaData, stream, FileMetaData.MimeType);
                         request.Fields = "id";
                         request.Upload();
                    }
                Permission perm = new Permission();
                perm.Type = "anyone";
                perm.Role = "reader";
                try
                {
                    service.Permissions.Create(perm, request.ResponseBody.Id).Execute(); //Creating Permission after folder creation.
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                }
                    var file1 = request.ResponseBody;
                    string[] tmp = file.FileName.Split('/');
                    linkDrive.Add(GetLinkFile(tmp[tmp.Length-1]));
                
                fileName.Add(tmp[1]);
            }
          }

        //delete folder googledrivefiles
        public static void DeleteFolder()
        {
        //F://tai_lieu_hoc_tap//01_Anh_Cuong//WebToanHoc//WebToanHoc//GoogleDriveFiles
            System.IO.DirectoryInfo di = new DirectoryInfo("F://tai_lieu_hoc_tap//01_Anh_Cuong//WebToanHoc_1//WebToanHoc//GoogleDriveFiles");
            if(di.Exists)
            {
                foreach (FileInfo file in di.EnumerateFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.EnumerateDirectories())
                {
                    dir.Delete(true);
                }
            }
            
        }
     }
}