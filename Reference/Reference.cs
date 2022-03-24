using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq; // Sort
using System.IO;
using System.Reflection; // Dynamic Loading
using System.Diagnostics; // External Program

namespace Reference
{
    class Reference
    {
        static void Main(string[] args)
        {
            // About File IO
            FileInputOutput();

            // About Sort
            LinqSort();

            // About Dynamic Loading
            DynamicLoading();

            // About External Process Execute
            ExternalProgramExecute();
        }

        static void FileInputOutput()
        {
            Console.WriteLine(Environment.NewLine + "=================================================");
            Console.WriteLine("==================== File IO ====================");
            Console.WriteLine("=================================================");

            string Project_Path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName; // 프로젝트 경로
            Console.WriteLine(Project_Path);

            FileIO fileIO = new FileIO();

            List<string> writedata = new List<string>();
            writedata.Add("fdasfdas");
            writedata.Add("4178947819jfviuafsd");
            fileIO.WriteFile("sample_write.txt", writedata);

            List<string> contents = fileIO.ReadFile(Project_Path + @"\sample.txt");
            foreach (string content in contents)
            {
                Console.WriteLine(content);
            }

            string target = "sample.txt";
            fileIO.Search(Project_Path, target);
        }
        public class FileIO
        {
            public List<string> ReadFile(string fileName)
            {
                List<string> list = new List<string>();

                StreamReader sr = new StreamReader(new FileStream(fileName, FileMode.Open));
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    list.Add(line);
                }
                sr.Close();

                return list;
            }

            public void WriteFile(string fileName, List<string> filedata)
            {
                StreamWriter sw = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate));

                foreach (string data in filedata)
                {
                    sw.WriteLine(data);
                }

                sw.Close();
            }

            public void Search(string path, string target)
            {
                string[] files = Directory.GetFiles(path, "", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    string[] paths = file.Split("\\");
                    if (target == paths[paths.Length - 1])
                    {
                        Console.WriteLine("Target Location : " + file);
                    }
                }
            }
        }

        static void LinqSort()
        {
            Console.WriteLine(Environment.NewLine + "=================================================");
            Console.WriteLine("=================== Linq Sort ===================");
            Console.WriteLine("=================================================");

            List<User> UserList = new List<User>();

            UserList.Add(new User("김", 13, 60.0));
            UserList.Add(new User("김", 23, 80.0));
            UserList.Add(new User("김", 23, 70.0));
            UserList.Add(new User("이", 17, 60.0));
            UserList.Add(new User("이", 37, 70.0));
            UserList.Add(new User("이", 27, 80.0));
            UserList.Add(new User("박", 34, 60.0));
            UserList.Add(new User("박", 64, 70.0));
            UserList.Add(new User("박", 54, 80.0));

            Console.WriteLine(Environment.NewLine + " === 이름순 정렬 ===");
            List<User> SortedByName = UserList.OrderBy(x => x.name).ToList();
            PrintUserList(SortedByName);

            Console.WriteLine(Environment.NewLine + " === 나이순 정렬 ===");
            List<User> SortedByAge = UserList.OrderBy(x => x.age).ToList();
            PrintUserList(SortedByAge);

            Console.WriteLine(Environment.NewLine + " === 몸무게순 정렬 ===");
            List<User> SortedByWeight = UserList.OrderBy(x => x.weight).ToList();
            PrintUserList(SortedByWeight);

            Console.WriteLine(Environment.NewLine + " === 이름순 정렬 후 -> 나이순 정렬 ( 2 key ) ===");
            List<User> SortedByNameAge = UserList.OrderBy(x => x.name).ThenBy(x => x.age).ToList();
            PrintUserList(SortedByNameAge);

            Console.WriteLine(Environment.NewLine + " === 이름순 정렬 후 -> 나이순 정렬 후 -> 몸무게순 정렬 ( 3 key ) ===");
            List<User> SortedByNameAgeWeight = UserList.OrderBy(x => x.name).ThenBy(x => x.age).ThenBy(x => x.weight).ToList();
            PrintUserList(SortedByNameAgeWeight);
        }
        static void PrintUserList(List<User> list)
        {
            foreach (var user in list)
            {
                Console.WriteLine(user);
            }
        }
        class User
        {
            public string name;
            public int age;
            public double weight;

            public User(string name, int age, double weight)
            {
                this.name = name;
                this.age = age;
                this.weight = weight;
            }

            public override string ToString()
            {
                return $"이름 : {name} / 나이 : {age} / 몸무게 : {weight}";
            }
        }

        static void DynamicLoading()
        {
            Console.WriteLine(Environment.NewLine + "=================================================");
            Console.WriteLine("================ Dynamic Loading ================");
            Console.WriteLine("=================================================");

            Assembly asm = Assembly.LoadFrom(@"C:\Users\pjh92\Desktop\IDE\dev\CSharp-Practice\Reference\Reference-dll"); // 방법 1
            string Project_Path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName; // 프로젝트 경로
            Console.WriteLine(Project_Path);
            // Assembly asm = Assembly.LoadFrom(Project_Path + @"\Reference-dll"); // 방법 2

            Type[] types = asm.GetExportedTypes();
            string classFullName = string.Empty;
            foreach (Type type in types)
            {
                Console.WriteLine(type.FullName);
                classFullName = type.FullName;
            }

            dynamic obj = Activator.CreateInstance(types[0]);

            obj.Sample();
        }

        static void ExternalProgramExecute()
        {
            Console.WriteLine(Environment.NewLine + "=================================================");
            Console.WriteLine("=============== External Program ================");
            Console.WriteLine("=================================================");

            ExternalProcess externalProcess = new ExternalProcess();
            externalProcess.SetExternalProcess("cmd.exe");
            string res1 = externalProcess.Execute("/c ipconfig /all");
            Console.WriteLine(res1);

            ExternalProcess externalProcess2 = new ExternalProcess();
            externalProcess2.SetExternalProcess("표준입력 받는 외부 프로그램");
            string res2 = externalProcess2.Execute();
            Console.WriteLine(res2);
        }
        public class ExternalProcess
        {
            ProcessStartInfo ProcessInfo;
            public void SetExternalProcess(string ProcessName)
            {
                ProcessInfo = new ProcessStartInfo();
                ProcessInfo.FileName = ProcessName;
                ProcessInfo.CreateNoWindow = true;
                ProcessInfo.UseShellExecute = false;
                ProcessInfo.RedirectStandardInput = true;
                ProcessInfo.RedirectStandardOutput = true;
                ProcessInfo.RedirectStandardError = true;
            }
            public string Execute(string arguments)
            {
                string result = string.Empty;
                try
                {
                    using (Process process = new Process())
                    {
                        ProcessInfo.Arguments = arguments;
                        process.StartInfo = ProcessInfo;
                        process.Start();

                        result = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return result;
            }

            public string Execute()
            {
                string result = string.Empty;
                try
                {
                    using (Process process = new Process())
                    {
                        process.StartInfo = ProcessInfo;
                        process.Start();

                        process.StandardInput.WriteLine("StandardInput");
                        process.StandardInput.Flush();
                        process.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return result;
            }
        }
    }
}
