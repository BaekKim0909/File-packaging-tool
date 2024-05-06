using File_packaging_tool.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace File_packaging_tool.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        // MCF  RayMarking RayMaster  RayDrilling  LWS-Pro  NPRF


        //绑定的变量


        //要打包的文件列表
        private ObservableCollection<string> _selectedFiles = new ObservableCollection<string>();
        private ObservableCollection<string> _showFiles = new ObservableCollection<string>();
        //打包的包名
        private string packageName;
        //打包地址
        private string packagePath;
        //复制的地址
        private string copyPath;
        //中英文切换
        private bool isEnglish;
        public string PackagePath
        {
            get
            {
                return packagePath;
            }
            set
            {
                packagePath = value;
                RaisePropertyChanged(() => PackagePath);
            }
        }
        public string CopyPath
        {
            get
            {
                return copyPath;
            }
            set
            {
                copyPath = value;
                RaisePropertyChanged(() => CopyPath);
            }
        }
        public string PackageName
        {
            get
            {
                return packageName;
            }
            set
            {
                packageName = value;
                RaisePropertyChanged(() => PackageName);
            }
        }
        public bool IsEnglish
        {
            get
            {
                return isEnglish;
            }
            set
            {
                isEnglish = value;
                RaisePropertyChanged(() => IsEnglish);
            }
        }       
        //已选文件
        public ObservableCollection<string> SelectedFiles
        {
            get { return _selectedFiles; }
            set
            {
                if (_selectedFiles != value)
                {
                    _selectedFiles = value;
                    RaisePropertyChanged(nameof(SelectedFiles));
                }
            }
        }
        public ObservableCollection<FileModel> MCF { get; set; } = new ObservableCollection<FileModel>();
        public ObservableCollection<FileModel> CorrectionWizard { get; set; } = new ObservableCollection<FileModel>();
        public ObservableCollection<FileModel> RayMarking { get; set; } = new ObservableCollection<FileModel>();
        public ObservableCollection<FileModel> RayMaster { get; set; } = new ObservableCollection<FileModel>();
        public ObservableCollection<FileModel> RM5D { get; set; } = new ObservableCollection<FileModel>();

        public ObservableCollection<FileModel> LWS { get; set; } = new ObservableCollection<FileModel>();
        public ObservableCollection<FileModel> NPRF { get; set; } = new ObservableCollection<FileModel>();
        public ObservableCollection<FileModel> RMC2 { get; set; } = new ObservableCollection<FileModel>();
        public ObservableCollection<FileModel> EssencialFiles { get; set; } = new ObservableCollection<FileModel>();

        public ObservableCollection<FileModel> AdditionalFiles { get; set; } = new ObservableCollection<FileModel>();

        public ObservableCollection<FileModel> SelectedFilesArray { get; set; } = new ObservableCollection<FileModel>();
        //绑定的方法
        public RelayCommand<FileModel> ToggleCheckBoxCommand { get; }
        public RelayCommand<FileModel> ShowCommand { get; }
        public RelayCommand PackageCommand { get; }
        public RelayCommand SelectPackagePathCommand { get; }
        public RelayCommand SelectCopyPathCommand { get; }
        public RelayCommand ClearCopyPathCommand { get; }
        public RelayCommand ClearPackagePathCommand { get; }
        public RelayCommand<FileModel> RemoveFileCommand { get; }
        public RelayCommand SwitchUserManualCommand { get; }

        //展示文件名
        public ObservableCollection<string> ShowFiles
        {
            get
            {
                return _showFiles;
            }
            set
            {
                ShowFiles = value;
                RaisePropertyChanged(()=>ShowFiles);
            }
        }
        public MainViewModel()
        {
            IsEnglish = false;
            Init();
            ToggleCheckBoxCommand = new RelayCommand<FileModel>(ToggleCheckBox);
            PackageCommand = new RelayCommand(Package);
            SelectPackagePathCommand = new RelayCommand(SelectPackagePath);
            SelectCopyPathCommand = new RelayCommand(SelectCopyPath);
            ClearCopyPathCommand = new RelayCommand(ClearCopyPath);
            ClearPackagePathCommand = new RelayCommand(ClearPackagePath);
            RemoveFileCommand = new RelayCommand<FileModel>(RemoveFile);
            SwitchUserManualCommand = new RelayCommand(SwitchUserManual);
            PackagePath = string.Empty;
            CopyPath = string.Empty;
        }

        // RemoveFileCommand Button.Command  ICommand 在类型为 MainWindow 的对象上找不到 RemoveFileCommand 属性。			
        //初始化方法
        public void Init()
        {
            CorrectionWizard.Clear();
            MCF.Clear();
            RayMarking.Clear();
            RayMaster.Clear();
            LWS.Clear();
            NPRF.Clear();
            RM5D.Clear();
            RMC2.Clear();
            EssencialFiles.Clear();
            AdditionalFiles.Clear();
            LoadItems(@"Z:\RMC2软件包\拷贝文件分组测试\CorrectionWizard", CorrectionWizard);
            LoadItems(@"Z:\RMC2软件包\拷贝文件分组测试\MCF", MCF);
            LoadItems(@"Z:\RMC2软件包\拷贝文件分组测试\RayMarking", RayMarking);
            LoadItems(@"Z:\RMC2软件包\拷贝文件分组测试\RayMaster", RayMaster);
            LoadItems(@"Z:\RMC2软件包\拷贝文件分组测试\LWS", LWS);
            LoadItems(@"Z:\RMC2软件包\拷贝文件分组测试\NPRF", NPRF);
            LoadItems(@"Z:\RMC2软件包\拷贝文件分组测试\RM5D", RM5D);
            LoadItems(@"Z:\RMC2软件包\拷贝文件分组测试\RMC2", RMC2);
            LoadItems(@"Z:\RMC2软件包\拷贝文件分组测试\必备文件程序包", EssencialFiles);
            LoadItems(@"Z:\RMC2软件包\拷贝文件分组测试\附加文件夹", AdditionalFiles);
        }
        //读取文件夹操作
        public void LoadItems(string path, ObservableCollection<FileModel> collection)
        {
            if (!Directory.Exists(path))
                return;

            foreach (string filePath in Directory.GetFiles(path))
            {
                var item = new FileModel
                {
                    FilePath = filePath,
                    FileName = Path.GetFileName(filePath),
                    IsSelect = false
                };
                collection.Add(item);
                if (IsPDF(item.FilePath))
                {
                    if (IsEnglish)
                    {
                        if (!item.FilePath.Contains("(EN)"))
                        {
                            collection.Remove(item);
                        }
                    }
                    else
                    {
                        if (item.FilePath.Contains("(EN)"))
                        {
                            collection.Remove(item);
                        }
                    }
                }
            }

            foreach (string directoryPath in Directory.GetDirectories(path))
            {
                var directoryItem = new FileModel
                {
                    FilePath = directoryPath,
                    FileName = Path.GetFileName(directoryPath),
                    IsSelect = false
                };
                LoadItems(directoryPath, directoryItem.Children);
                collection.Add(directoryItem);
            }

        }
        //复选框事件
        public void ToggleCheckBox(FileModel file)
        {
            if (file == null)
                return;
            ToggleChildrenIsSelect(file);
            if (SelectedFiles.Contains(file.FilePath))
            {
                SelectedFilesArray.Remove(file);
                SelectedFiles.Remove(file.FilePath);
                file.IsSelect = false;
            }
            else
            {
                SelectedFilesArray.Add(file);
                SelectedFiles.Add(file.FilePath);
                file.IsSelect = true;
            }
        }
        //
        public void ToggleChildrenIsSelect(FileModel file)
        {
            if (file == null)
                return;

            foreach (var child in file.Children)
            {
                child.IsSelect = !child.IsSelect; // 将子节点的 IsSelect 属性取反

                // 递归调用以处理子节点的子节点
                ToggleChildrenIsSelect(child);
            }
        }
        // Funciton 打包功能
        public void Package()
        {
            if (string.IsNullOrEmpty(PackageName))
            {
                System.Windows.MessageBox.Show("请输入包名");
                return;
            }
            SelectedFiles.Clear();
            foreach(var file in SelectedFilesArray)
            {
                SelectedFiles.Add(file.FilePath.ToString());
            }
            // 创建一个新的压缩包（打包到指定位置）
            if (!string.IsNullOrEmpty(PackagePath))
            {
                string zipFilePath = Path.Combine(PackagePath, PackageName += ".zip"); // 压缩包的路径和文件名

                using (FileStream zipFileStream = new FileStream(zipFilePath, FileMode.Create))
                {
                    using (ZipArchive archive = new ZipArchive(zipFileStream, ZipArchiveMode.Create))
                    {
                        foreach (string sourcePath in SelectedFiles)
                        {
                            if (File.Exists(sourcePath))
                            {
                                // 添加文件到压缩包中
                                string fileName = Path.GetFileName(sourcePath);
                                archive.CreateEntryFromFile(sourcePath, fileName);
                            }
                            else if (Directory.Exists(sourcePath))
                            {
                                // 添加文件夹及其所有内容到压缩包中
                                string sourceDirectoryName = new DirectoryInfo(sourcePath).Name;
                                string entryName = Path.Combine(sourceDirectoryName, ""); // 将文件夹添加进压缩包，后面的空字符串表示文件夹路径
                                AddDirectoryToZip(archive, sourcePath, entryName);
                            }
                            else
                            {
                                Console.WriteLine($"文件或文件夹 {sourcePath} 不存在，无法添加到压缩包中。");
                            }
                        }
                    }
                }

                Console.WriteLine($"压缩包已成功创建：{zipFilePath}");
                System.Windows.MessageBox.Show($"压缩包已成功创建：{zipFilePath}");

            }
            if (CopyPath != string.Empty)
            {
                foreach (string path in _selectedFiles)
                {
                    if (File.Exists(path))
                    {
                        // 如果路径是文件，则复制文件到目标目录
                        CopyFile(path, CopyPath);
                    }
                    else if (Directory.Exists(path))
                    {
                        // 如果路径是文件夹，则复制文件夹及其内容到目标目录
                        CopyDirectory(path, CopyPath);
                    }
                }
            }
            PackageName = string.Empty;

        }
        public void SelectPackagePath()
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                // 设置对话框的描述文本
                folderBrowserDialog.Description = "请选择文件夹";

                // 显示对话框，并等待用户选择文件夹
                DialogResult result = folderBrowserDialog.ShowDialog();

                // 检查用户是否点击了“确定”按钮
                if (result == DialogResult.OK)
                {
                    // 获取用户选择的文件夹路径
                    PackagePath = folderBrowserDialog.SelectedPath;
                }
            }
        }
        public void SelectCopyPath()
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                // 设置对话框的描述文本
                folderBrowserDialog.Description = "请选择文件夹";

                // 显示对话框，并等待用户选择文件夹
                DialogResult result = folderBrowserDialog.ShowDialog();

                // 检查用户是否点击了“确定”按钮
                if (result == DialogResult.OK)
                {
                    // 获取用户选择的文件夹路径
                    CopyPath = folderBrowserDialog.SelectedPath;
                }
            }
        }
        //删除对应文件
        public void RemoveFile(FileModel file)
        {
            file.IsSelect = false;
            SelectedFilesArray.Remove(file);
            SelectedFiles.Remove(file.FilePath);
            foreach (var child in file.Children)
            {
                child.IsSelect = !child.IsSelect; // 将子节点的 IsSelect 属性取反

                // 递归调用以处理子节点的子节点
                ToggleChildrenIsSelect(child);
            }
            Console.WriteLine(file);
        }
        //置空复制路径
        public void ClearCopyPath()
        {
            CopyPath = string.Empty;
        }
        //置空打包路径
        public void ClearPackagePath()
        {
            PackagePath = string.Empty;
        }
        //Function 复制文件
        public void CopyFile(string sourceFilePath, string targetDir)
        {
            // 获取文件名
            string fileName = Path.GetFileName(sourceFilePath);

            // 拼接目标文件路径
            string destinationPath = Path.Combine(targetDir, fileName);

            // 复制文件
            File.Copy(sourceFilePath, destinationPath);
        }
        //Function 复制文件夹
        private void CopyDirectory(string sourceDirectoryPath, string destinationPath)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceDirectoryPath);
            string destinationDirectoryPath = Path.Combine(destinationPath, sourceDirectory.Name);
            Directory.CreateDirectory(destinationDirectoryPath); // 创建目标文件夹

            foreach (string filePath in Directory.GetFiles(sourceDirectoryPath))
            {
                string fileName = Path.GetFileName(filePath);
                string destinationFilePath = Path.Combine(destinationDirectoryPath, fileName);
                File.Copy(filePath, destinationFilePath, true); // 设置 overwrite 参数为 true，表示允许覆盖同名文件
            }

            foreach (string subDirectoryPath in Directory.GetDirectories(sourceDirectoryPath))
            {
                CopyDirectory(subDirectoryPath, destinationDirectoryPath);
            }
        }
        public void AddDirectoryToZip(ZipArchive archive, string sourceDirectoryPath, string entryName)
        {
            // 添加文件夹本身到压缩包
            archive.CreateEntry(entryName + "/"); // 使用 "/" 表示文件夹路径
            foreach (string filePath in Directory.GetFiles(sourceDirectoryPath))
            {
                string fileName = Path.GetFileName(filePath);
                string entryPath = Path.Combine(entryName, fileName);
                archive.CreateEntryFromFile(filePath, entryPath);
            }

            foreach (string subDirectoryPath in Directory.GetDirectories(sourceDirectoryPath))
            {
                string subDirectoryName = new DirectoryInfo(subDirectoryPath).Name;
                string entryPath = Path.Combine(entryName, subDirectoryName, ""); // 将子文件夹添加进压缩包，后面的空字符串表示子文件夹路径
                AddDirectoryToZip(archive, subDirectoryPath, entryPath);
            }
        }
        public bool IsPDF(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            return string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase);
        }
        public void SwitchUserManual()
        {
            Init();
            for (int i = 0; i < SelectedFilesArray.Count; i++)
            {
                var file = SelectedFilesArray[i];
                if (IsPDF(file.FilePath))
                {
                    if (IsEnglish)
                    {
                        if (!file.FilePath.Contains("(EN)"))
                        {
                           SelectedFilesArray.Remove(file);
                           file.IsSelect = false;
                           i--;
                        }
                    }
                    else
                    {
                        if (file.FilePath.Contains("(EN)"))
                        {
                            SelectedFilesArray.Remove(file);
                            file.IsSelect= false;
                            i--;
                        }
                    }
                }
            }
        }
    }
}