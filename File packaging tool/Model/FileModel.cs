using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_packaging_tool.Model
{
    public class FileModel : ObservableObject
    {
        private string fileName;
        private string filePath;
        private bool isSelect;        
        public string FileName
        {
            get { return fileName; }
            set { 
                fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                RaisePropertyChanged(() => FilePath);
            }
        }
        public bool IsSelect
        {
            get { return isSelect; }
            set
            {
                isSelect = value;
                RaisePropertyChanged(() => IsSelect);
            }
        }

        public ObservableCollection<FileModel> Children { get; } = new ObservableCollection<FileModel>();
    }
}
