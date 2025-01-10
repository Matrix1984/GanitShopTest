namespace GanitShop.Helpers
{
    public static class FileNameGenerator
    {
        public  static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString()
                      + Path.GetExtension(fileName);
        }
    }
}
