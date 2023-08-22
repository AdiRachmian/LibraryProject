using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using Windows.Storage;

namespace Logic
{
    public class IOOperations
    {
        public static async void SaveData(Manager mngr)
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            string jsonString = JsonSerializer.Serialize(mngr);

            StorageFile libraryFile = await localFolder.CreateFileAsync("LibraryData.json",
                CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(libraryFile, jsonString);
        }

        public static async void LoadDataAsync(Manager mngr)
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            string text =
                await FileIO.ReadTextAsync(await localFolder.GetFileAsync("LibraryData.json"));

            using (var stream = GenerateStreamFromString(text))
            {
                Manager manager =
                     await JsonSerializer.DeserializeAsync<Manager>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                mngr = manager;
            }
        }



        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
