

namespace FiorelloSlider_OnetoMany.Helpers.Extentions
{
    public static class FlieExtention
    {
        public static bool CheckFileSize(this IFormFile file,int size)
        {
            return file.Length/1024 < size;
        }

        public static bool CheckFileType(this IFormFile file,string pattern) 
        { 
            return file.ContentType.Contains(pattern);
        }

        public async static Task SaveFileToLocalAsync(this IFormFile file,string path)
        {
            using FileStream stream = new(path, FileMode.Create);
            await file.CopyToAsync(stream);
        }

        public static void DeleteFileFromLocal(this string path)
        {
            if(File.Exists(path))
              File.Delete(path);
        }
    }
}
