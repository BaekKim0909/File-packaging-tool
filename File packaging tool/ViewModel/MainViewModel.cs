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


        //�󶨵ı���


        //Ҫ������ļ��б�
        private ObservableCollection<string> _selectedFiles = new ObservableCollection<string>();
        private ObservableCollection<string> _showFiles = new ObservableCollection<string>();
        //����İ���
        private string packageName;
        //�����ַ
        private string packagePath;
        //���Ƶĵ�ַ
        private string copyPath;
        //��Ӣ���л�
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
        //��ѡ�ļ�
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
        //�󶨵ķ���
        public RelayCommand<FileModel> ToggleCheckBoxCommand { get; }
        public RelayCommand<FileModel> ShowCommand { get; }
        public RelayCommand PackageCommand { get; }
        public RelayCommand SelectPackagePathCommand { get; }
        public RelayCommand SelectCopyPathCommand { get; }
        public RelayCommand ClearCopyPathCommand { get; }
        public RelayCommand ClearPackagePathCommand { get; }
        public RelayCommand<FileModel> RemoveFileCommand { get; }
        public RelayCommand SwitchUserManualCommand { get; }

        //չʾ�ļ���
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

        // RemoveFileCommand Button.Command  ICommand ������Ϊ MainWindow �Ķ������Ҳ��� RemoveFileCommand ���ԡ�			
        //��ʼ������
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
            LoadItems(@"Z:\RMC2�����\�����ļ��������\CorrectionWizard", CorrectionWizard);
            LoadItems(@"Z:\RMC2�����\�����ļ��������\MCF", MCF);
            LoadItems(@"Z:\RMC2�����\�����ļ��������\RayMarking", RayMarking);
            LoadItems(@"Z:\RMC2�����\�����ļ��������\RayMaster", RayMaster);
            LoadItems(@"Z:\RMC2�����\�����ļ��������\LWS", LWS);
            LoadItems(@"Z:\RMC2�����\�����ļ��������\NPRF", NPRF);
            LoadItems(@"Z:\RMC2�����\�����ļ��������\RM5D", RM5D);
            LoadItems(@"Z:\RMC2�����\�����ļ��������\RMC2", RMC2);
            LoadItems(@"Z:\RMC2�����\�����ļ��������\�ر��ļ������", EssencialFiles);
            LoadItems(@"Z:\RMC2�����\�����ļ��������\�����ļ���", AdditionalFiles);
        }
        //��ȡ�ļ��в���
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
        //��ѡ���¼�
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
                child.IsSelect = !child.IsSelect; // ���ӽڵ�� IsSelect ����ȡ��

                // �ݹ�����Դ����ӽڵ���ӽڵ�
                ToggleChildrenIsSelect(child);
            }
        }
        // Funciton �������
        public void Package()
        {
            if (string.IsNullOrEmpty(PackageName))
            {
                System.Windows.MessageBox.Show("���������");
                return;
            }
            SelectedFiles.Clear();
            foreach(var file in SelectedFilesArray)
            {
                SelectedFiles.Add(file.FilePath.ToString());
            }
            // ����һ���µ�ѹ�����������ָ��λ�ã�
            if (!string.IsNullOrEmpty(PackagePath))
            {
                string zipFilePath = Path.Combine(PackagePath, PackageName += ".zip"); // ѹ������·�����ļ���

                using (FileStream zipFileStream = new FileStream(zipFilePath, FileMode.Create))
                {
                    using (ZipArchive archive = new ZipArchive(zipFileStream, ZipArchiveMode.Create))
                    {
                        foreach (string sourcePath in SelectedFiles)
                        {
                            if (File.Exists(sourcePath))
                            {
                                // ����ļ���ѹ������
                                string fileName = Path.GetFileName(sourcePath);
                                archive.CreateEntryFromFile(sourcePath, fileName);
                            }
                            else if (Directory.Exists(sourcePath))
                            {
                                // ����ļ��м����������ݵ�ѹ������
                                string sourceDirectoryName = new DirectoryInfo(sourcePath).Name;
                                string entryName = Path.Combine(sourceDirectoryName, ""); // ���ļ�����ӽ�ѹ����������Ŀ��ַ�����ʾ�ļ���·��
                                AddDirectoryToZip(archive, sourcePath, entryName);
                            }
                            else
                            {
                                Console.WriteLine($"�ļ����ļ��� {sourcePath} �����ڣ��޷���ӵ�ѹ�����С�");
                            }
                        }
                    }
                }

                Console.WriteLine($"ѹ�����ѳɹ�������{zipFilePath}");
                System.Windows.MessageBox.Show($"ѹ�����ѳɹ�������{zipFilePath}");

            }
            if (CopyPath != string.Empty)
            {
                foreach (string path in _selectedFiles)
                {
                    if (File.Exists(path))
                    {
                        // ���·�����ļ��������ļ���Ŀ��Ŀ¼
                        CopyFile(path, CopyPath);
                    }
                    else if (Directory.Exists(path))
                    {
                        // ���·�����ļ��У������ļ��м������ݵ�Ŀ��Ŀ¼
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
                // ���öԻ���������ı�
                folderBrowserDialog.Description = "��ѡ���ļ���";

                // ��ʾ�Ի��򣬲��ȴ��û�ѡ���ļ���
                DialogResult result = folderBrowserDialog.ShowDialog();

                // ����û��Ƿ����ˡ�ȷ������ť
                if (result == DialogResult.OK)
                {
                    // ��ȡ�û�ѡ����ļ���·��
                    PackagePath = folderBrowserDialog.SelectedPath;
                }
            }
        }
        public void SelectCopyPath()
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                // ���öԻ���������ı�
                folderBrowserDialog.Description = "��ѡ���ļ���";

                // ��ʾ�Ի��򣬲��ȴ��û�ѡ���ļ���
                DialogResult result = folderBrowserDialog.ShowDialog();

                // ����û��Ƿ����ˡ�ȷ������ť
                if (result == DialogResult.OK)
                {
                    // ��ȡ�û�ѡ����ļ���·��
                    CopyPath = folderBrowserDialog.SelectedPath;
                }
            }
        }
        //ɾ����Ӧ�ļ�
        public void RemoveFile(FileModel file)
        {
            file.IsSelect = false;
            SelectedFilesArray.Remove(file);
            SelectedFiles.Remove(file.FilePath);
            foreach (var child in file.Children)
            {
                child.IsSelect = !child.IsSelect; // ���ӽڵ�� IsSelect ����ȡ��

                // �ݹ�����Դ����ӽڵ���ӽڵ�
                ToggleChildrenIsSelect(child);
            }
            Console.WriteLine(file);
        }
        //�ÿո���·��
        public void ClearCopyPath()
        {
            CopyPath = string.Empty;
        }
        //�ÿմ��·��
        public void ClearPackagePath()
        {
            PackagePath = string.Empty;
        }
        //Function �����ļ�
        public void CopyFile(string sourceFilePath, string targetDir)
        {
            // ��ȡ�ļ���
            string fileName = Path.GetFileName(sourceFilePath);

            // ƴ��Ŀ���ļ�·��
            string destinationPath = Path.Combine(targetDir, fileName);

            // �����ļ�
            File.Copy(sourceFilePath, destinationPath);
        }
        //Function �����ļ���
        private void CopyDirectory(string sourceDirectoryPath, string destinationPath)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceDirectoryPath);
            string destinationDirectoryPath = Path.Combine(destinationPath, sourceDirectory.Name);
            Directory.CreateDirectory(destinationDirectoryPath); // ����Ŀ���ļ���

            foreach (string filePath in Directory.GetFiles(sourceDirectoryPath))
            {
                string fileName = Path.GetFileName(filePath);
                string destinationFilePath = Path.Combine(destinationDirectoryPath, fileName);
                File.Copy(filePath, destinationFilePath, true); // ���� overwrite ����Ϊ true����ʾ������ͬ���ļ�
            }

            foreach (string subDirectoryPath in Directory.GetDirectories(sourceDirectoryPath))
            {
                CopyDirectory(subDirectoryPath, destinationDirectoryPath);
            }
        }
        public void AddDirectoryToZip(ZipArchive archive, string sourceDirectoryPath, string entryName)
        {
            // ����ļ��б���ѹ����
            archive.CreateEntry(entryName + "/"); // ʹ�� "/" ��ʾ�ļ���·��
            foreach (string filePath in Directory.GetFiles(sourceDirectoryPath))
            {
                string fileName = Path.GetFileName(filePath);
                string entryPath = Path.Combine(entryName, fileName);
                archive.CreateEntryFromFile(filePath, entryPath);
            }

            foreach (string subDirectoryPath in Directory.GetDirectories(sourceDirectoryPath))
            {
                string subDirectoryName = new DirectoryInfo(subDirectoryPath).Name;
                string entryPath = Path.Combine(entryName, subDirectoryName, ""); // �����ļ�����ӽ�ѹ����������Ŀ��ַ�����ʾ���ļ���·��
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