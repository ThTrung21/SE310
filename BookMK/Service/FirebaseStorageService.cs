using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BookMK.Service
{
    public class FirebaseStorageService
    {
        private readonly string _bucketName = "gs://bookmk-a5859.appspot.com";
        private readonly StorageClient _storageClient;
        private readonly FirebaseStorageService _firebaseStorage;
        
        public FirebaseStorageService()
        {
            
            _storageClient = StorageClient.Create();
        }
       
        public async Task<string> UploadImageAsync(string filePath, string fileName)
        {
            using (var fileStream = File.OpenRead(filePath))
            {
                var uploadObject = await _storageClient.UploadObjectAsync(
                    _bucketName,
                    fileName,
                    null,
                    fileStream);

                return uploadObject.MediaLink; // Returns the public URL for the uploaded file
            }
        }
    }
}
