using System.IO;
namespace OpenSBP {
    public struct FileInfo {
        public FileStream fs;
        public StreamReader Reader;
        public StreamWriter Writer;
        public string Mode;
    }
}
