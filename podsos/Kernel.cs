using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace podsos
{
    
        


namespace MyPonOS
    {
        public class Kernel 
        {
            public static string disk = Environment.CurrentDirectory + @"\";
            public static string dirinsubsystem = @"root\";
            public static string pathtouserconf = disk + @"UserData\user.conf";
            public static string pathtosysconf = disk + @"System\boot.conf";
            public static string pathtosysvaribles = disk + @"System\list.var";
            public static string pathtosystem = disk + @"System";
            public static string pathtohome = disk + @"home";
            public static string pathtouserdata = disk + @"UserData";
            public static string directoryPath = disk + @"home";
            public static List<string> var = new List<string>();
            public static string password = "";
            public static string pathtodisk = "";
            public static string username = "user";
            public static string os = "subos ponOS CL-build 1.1";
            public static string pcname = Environment.MachineName;
            public static string osuser = Environment.UserName;
            public static bool enabledby = false;
            public static bool sudo = false;


            public static void Checkfolder()
            {

                if (!System.IO.Directory.Exists(pathtosystem))
                {
                    System.IO.Directory.CreateDirectory(pathtosystem);
                    Console.WriteLine("System directory created.");
                }
                if (!System.IO.File.Exists(pathtosysconf))
                {


                    File.WriteAllText(pathtosysconf, $"systemcotolog={Environment.CurrentDirectory}\\");



                }
                if (!System.IO.Directory.Exists(pathtouserdata))
                {

                    System.IO.Directory.CreateDirectory(pathtouserdata);
                    Console.WriteLine("User directory created.");
                    Console.Write("Write your username>");
                    string user = Console.ReadLine();

                    Console.Write("Write your password>");
                    string pass = Console.ReadLine();
                    Console.Write("Enabled by file(without this package some functions may not work)?(y/n)>");
                    string inter = Console.ReadLine();
                    if (user != null && user != "")
                    {

                        System.IO.File.Create(pathtouserconf).Close();
                        System.IO.File.WriteAllText(pathtouserconf, $"user={user}\npassword={pass}");
                        if (inter == "y")
                        {
                            System.IO.File.WriteAllText(pathtouserconf, $"user={user}\npassword={pass}\nenabled_by=true");
                        }

                    }


                }
                if (!System.IO.Directory.Exists(pathtohome))
                {
                    System.IO.Directory.CreateDirectory(pathtohome);
                    Console.WriteLine("Home directory created.");
                }
                if (!System.IO.File.Exists(pathtosysvaribles))
                {
                    System.IO.File.WriteAllText(pathtosysvaribles, "bypin=start\nroot\\="+disk);
                }


            }

            public static void BeforeRun()
            {


                Console.WriteLine("Welcome to PonOS!");
                
                Console.WriteLine("Filesystem initialized.");



                Console.WriteLine("ver-ponos");


            }


            
            public static void Run()
            {
                try
                {


                    Checkfolder();
                    readvar(pathtosysvaribles);
                    readconf(pathtouserconf);
                    readconf(pathtosysconf);

                    string User = username;
                    foreach (string line in var)
                    {

                        string name = line.Split('|').First();
                        string arg = line.Split('|').Last();

                        if (directoryPath.Contains(arg))
                        {
                            if (sudo == true)
                            {
                                Console.Write($"root({directoryPath.Replace(arg,name)})>");
                                dirinsubsystem = directoryPath.Replace(arg, name);
                                break;
                            }
                            else
                            {
                                Console.Write($"{User}({directoryPath.Replace(arg, name)})>");
                                dirinsubsystem = directoryPath.Replace(arg, name);
                                break;
                            }
                        }
                    }
                   


                    string input = Console.ReadLine();
                    foreach (string line in var)
                    {

                        string name = line.Split('|').First();
                        string arg = line.Split('|').Last();

                        if (input.Contains(name))
                        {
                            input = input.Replace(name, arg);
                        }
                    }



                    if (input == "time")
                    {
                        Console.WriteLine("Current time: " + DateTime.Now.ToString("HH:mm:ss"));
                    }
                    
                    else if (input.StartsWith("start "))
                    {
                        string path = input.Substring(6);
                        Console.WriteLine("start file:" + path);
                        startfile(path);

                    }
                    else if (input == "sudo")
                    {
                        if (password != "")
                        {
                            Console.Write("password>");
                            string pass = Console.ReadLine();
                            if (pass == password)
                            {
                                sudo = true;
                            }
                            else
                            {
                                Console.WriteLine("access denied");
                            }
                        }
                        else
                        {
                            sudo = true;
                        }
                    }

                    else if (input.StartsWith("cd "))
                    {
                        string path = input.Substring(3);
                        if (System.IO.Directory.Exists(path))
                        {
                            Console.WriteLine(path);
                            directoryPath = path;
                        }

                        else
                        {
                            Console.WriteLine("Not found");
                        }
                    }
                    else if (input.StartsWith("readfile "))
                    {

                        string path = input.Substring(9);
                        if (System.IO.File.Exists(path))
                        {

                            Console.WriteLine(System.IO.File.ReadAllText(path));

                        }

                        else
                        {
                            Console.WriteLine("Not found");
                        }

                    }
                    else if (input.StartsWith("writefile "))
                    {

                        string path = input.Substring(10);

                        Console.Write("how many lines do you want to write?>");
                        string countlines = Console.ReadLine();
                        int count = 1;
                        try
                        {
                            count = Convert.ToInt32(countlines);
                        }
                        catch (Exception e) { Console.WriteLine(e); }

                        string text = "";
                        for (int i = 0; i <= count; i++)
                        {
                            Console.Write("text>");
                            string inp = Console.ReadLine();
                            text = text + $"{inp}\n";
                        }

                        if (!System.IO.File.Exists(path))
                        {
                            System.IO.File.Create(path).Close();
                        }
                        System.IO.File.WriteAllText(path, text);

                    }

                    else if (input.StartsWith("mkdir "))
                    {
                        string path = input.Substring(6);
                        Console.WriteLine(path);
                        directoryPath = directoryPath + @"\" + path;
                        if (!System.IO.Directory.Exists(directoryPath))
                        {
                            System.IO.Directory.CreateDirectory(directoryPath);
                        }
                        else
                        {
                            Console.WriteLine("Already created");
                        }
                    }
                    else if (input.StartsWith("rmdir "))
                    {
                        string path = input.Substring(6);

                        Console.WriteLine(path);

                        if (System.IO.Directory.Exists(path))
                        {
                            if (sudo == false)
                            {


                                Console.WriteLine("access denied");


                            }
                            else
                            {
                                System.IO.Directory.Delete(path);
                                directoryPath = disk;
                            }




                        }

                        else
                        {
                            Console.WriteLine("Not found");
                        }


                    }
                    else if (input == "dir")
                    {
                        foreach (string path in System.IO.Directory.GetDirectories(directoryPath))
                        {
                            Console.WriteLine(path);
                        }
                        foreach (string path in System.IO.Directory.GetFiles(directoryPath))
                        {
                            Console.WriteLine(path);
                        }
                    }
                    else if (input.StartsWith("mkfile "))
                    {
                        string path = input.Substring(7);
                        path = directoryPath + @"\" + path;
                        if (!System.IO.File.Exists(path))
                        {
                            System.IO.File.Create(path).Close();
                        }
                        else
                        {
                            Console.WriteLine("Already created");
                        }

                    }
                    else if (input.StartsWith("rmfile "))
                    {
                        string path = input.Substring(7);
                        string exitpath = directoryPath + @"\" + path;
                        Console.WriteLine(exitpath);

                        if (File.Exists(exitpath))
                        {

                            File.Delete(exitpath);

                        }

                        else
                        {
                            Console.WriteLine("Not found");
                        }

                    }
                    else if (input == "chdata")
                    {
                        if (sudo)
                        {

                            if (System.IO.File.Exists(pathtouserconf))
                            {
                                System.IO.File.Delete(pathtouserconf);
                            }
                            if (System.IO.Directory.Exists(pathtouserdata))
                            {
                                System.IO.Directory.Delete(pathtouserdata);
                            }
                            Init.reboot();
                        }
                        else
                        {
                            Console.WriteLine("access denied");
                        }
                    }
                    else if (input == "reboot")
                    {
                        Init.reboot();
                    }
                    else if (input == "exit")
                    {

                        Init.shutdown();
                    }
                    else if (input == "clear")
                    {
                        Console.Clear();
                    }
                    else
                    {

                        Console.WriteLine("Unknown command");
                    }
                }
                catch (Exception ex)
                {
                    
                    
                    
                    Console.Beep();
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Cyan;

                    Console.ForegroundColor = ConsoleColor.Red;
                    string bsod = " :(  \r\n                         \r\n A problem has been detected and ponos has been shut down to prevent damage\r\n to your computer.\r\n\r\n The problem seems to be caused by the following file: (Error description)\r\n\r\n *** STOP: 0x0000007B (0xFFFFFA80074A8B30, 0xFFFFFFFFC0000034, 0x0000000000000000, 0x0000000000000000)\r\n\r\n  Collecting data for crash dump...\r\n  Initializing disk for crash dump...\r\n  Dumping physical memory to disk: 100\r\n  Physical memory dump complete.\r\n  Contact your system administrator or technical support group for further assistance.";
                    Console.WriteLine(bsod + "\n" + ex);
                    Thread.Sleep(5000);
                    Init.reboot();
                }
            }
            public static void readconf(string path)
            {
                if (System.IO.File.Exists(path))
                {
                    foreach (string line in System.IO.File.ReadAllLines(path))
                    {
                        string setting = line.Split('=').First();
                        string arg = line.Split('=').Last();
                        setting = setting.Replace(" ", "");
                        arg = arg.Replace(" ", "");

                        if (setting == "password")
                        {
                            password = arg;
                        }
                        else if (setting == "user")
                        {
                            username = arg;
                        }
                        else if (setting == "start_up_sudo")
                        {
                            if (arg == "true")
                            {
                                sudo = true;
                            }

                        }
                        else if (setting == "enabled_by")
                        {
                            if (arg == "true")
                            {
                                enabledby = true;
                            }
                        }
                        else if(setting == "systemcotolog")
                        {
                            pathtodisk = arg;
                        }
                    }
                }

            }
            public static void readvar(string path)
            {
                if (System.IO.File.Exists(path))
                {
                    foreach (string line in System.IO.File.ReadAllLines(path))
                    {
                        string setting = line.Split('=').First();
                        string arg = line.Split('=').Last();
                        setting = setting.Replace(" ", "");
                        arg = arg.Replace(" ", "");

                        if (setting != "")
                        {

                            var.Add($"{setting}|{arg}");
                        }


                    }
                }

            }
            public static void startfile(string path)
            {
                if (enabledby == true)
                {
                    interAsync(path);
                }

            }
            public static async Task interAsync(string userfile = "")
            {



                string error = "";









                if (System.IO.File.Exists(userfile) == true && userfile.Contains(".by") == true)
                {

                    string[] lines = System.IO.File.ReadAllLines(userfile);

                    error = "";
                    bool noterror = false;

                    string var = "";
                    string userpath = "Hello World!";
                    int linescount = 1;
                    int ints = 0;
                    string inputuser = "";
                    foreach (string line in lines)
                    {



                        string[] words = line.Split(';');

                        foreach (string word in words)
                        {
                            if (word != null)
                            {
                                // переменные для команд
                                string commands = word.ToLower();
                                string[] argcom = commands.Split('(');
                                string arg = "";
                                string command = "";


                                // переменные для юзера

                                string user = username;
                                
                                string pcname = "asd";
                                string videocontroller = "video";
                                string motherboard = "motherboard";





                                foreach (string str in argcom)
                                {
                                    if (str.Contains(")") == true)
                                    {

                                        arg = str.TrimEnd(')');

                                    }
                                    else
                                    {
                                        command = str;
                                    }
                                }


                                string arg2 = arg.Split(new char[] { ',' }).Last();
                                arg = arg.Replace($",{arg2}", "");


                                if (arg != "")
                                {

                                    while (arg.Contains("{user}") == true || arg.Contains("{os}") == true || arg.Contains("{pcname}") == true || arg.Contains("{path}") == true || arg.Contains("{var}") == true || arg.Contains("{\\n}") == true || arg.Contains("{input}") == true|| arg.Contains("{math}") == true)
                                    {

                                        if (arg.Contains("{user}") == true)
                                        {
                                            arg = arg.Replace("{user}", $"{user}");
                                        }
                                        else if (arg.Contains("{math}") == true)
                                        {
                                            arg = arg.Replace("{math}", $"{ints}");
                                        }
                                        else if (arg.Contains("{os}") == true)
                                        {
                                            arg = arg.Replace("{os}", $"{os}");
                                        }
                                        else if (arg.Contains("{pcname}") == true)
                                        {
                                            arg = arg.Replace("{pcname}", $"{pcname}");
                                        }
                                        else if (arg.Contains("{video}") == true)
                                        {
                                            arg = arg.Replace("{video}", $"{videocontroller}");
                                        }
                                        else if (arg.Contains("{pcmodel}") == true)
                                        {
                                            arg = arg.Replace("{pcmodel}", $"{motherboard}");
                                        }

                                        else if (arg.Contains("{path}"))
                                        {
                                            arg = arg.Replace("{path}", userpath);
                                        }
                                        else if (arg.Contains("{input}"))
                                        {
                                            arg = arg.Replace("{input}", inputuser);
                                        }

                                        else if (arg.Contains("{var}") == true)
                                        {
                                            arg = arg.Replace("{var}", var);

                                        }
                                        else if (arg.Contains("{\\n}") == true)
                                        {
                                            string k = "";

                                            string[] d = arg.Split('\\');
                                            foreach (string s in d)
                                            {

                                                k = k + "\n" + s.Replace("{", "").Replace("n}", "");
                                            }
                                            arg = k;

                                        }
                                    }
                                    if (arg2 != "")
                                    {

                                        while (arg2.Contains("{user}") == true || arg2.Contains("{os}") == true || arg2.Contains("{pcname}") == true || arg2.Contains("{path}") == true || arg2.Contains("{var}") == true || arg2.Contains("{\\n}") == true || arg2.Contains("{input}") == true||arg2.Contains("{math}") == true)
                                        {

                                            if (arg2.Contains("{user}") == true)
                                            {
                                                arg2 = arg2.Replace("{user}", $"{user}");
                                            }
                                            else if (arg2.Contains("{math}") == true)
                                            {
                                                arg2 = arg2.Replace("{math}", $"{ints}");
                                            }
                                            else if (arg2.Contains("{os}") == true)
                                            {
                                                arg2 = arg2.Replace("{os}", $"{os}");
                                            }
                                            else if (arg2.Contains("{pcname}") == true)
                                            {
                                                arg2 = arg2.Replace("{pcname}", $"{pcname}");
                                            }
                                            else if (arg2.Contains("{video}") == true)
                                            {
                                                arg2 = arg2.Replace("{video}", $"{videocontroller}");
                                            }
                                            else if (arg2.Contains("{path}"))
                                            {
                                                arg2 = arg2.Replace("{path}", userpath);
                                            }
                                            else if (arg2.Contains("{input}"))
                                            {
                                                arg2 = arg2.Replace("{input}", inputuser);
                                            }
                                            else if (arg2.Contains("{var}") == true)
                                            {
                                                arg2 = arg2.Replace("{var}", var);

                                            }
                                            else if (arg2.Contains("{\\n}") == true)
                                            {
                                                string k = "";

                                                string[] d = arg2.Split('\\');
                                                foreach (string s in d)
                                                {

                                                    k = k + "\n" + s.Replace("{", "").Replace("n}", "");
                                                }
                                                arg2 = k;


                                            }
                                        }
                                    }




                                }
                                //команды
                                command = command.Replace(" ", "");

                                if (command == "clear")
                                {
                                    Console.Clear();
                                }
                                else if (command == "writefile")
                                {

                                    if (arg2 != "" && arg != "")
                                    {
                                        System.IO.File.WriteAllText(arg, arg2);
                                    }
                                    else { error = error + "\n" + $"в строке {linescount} допущена ошибка в слове {commands}:нет аргумента"; }
                                }
                                
                                    
                                else if (command == "readfile")
                                {
                                    if (System.IO.File.Exists(arg) == true)
                                    {
                                        userpath = System.IO.File.ReadAllText(arg);
                                    }
                                    else { error = error + "\n" + $"в строке {linescount} допущена ошибка в слове {commands}:файл не существует"; }
                                }
                                else if (command == "var")
                                {
                                    var = arg2;


                                }
                                else if (command == "write")
                                {
                                    Console.WriteLine(arg);
                                }
                                
                                else if (command == "input")
                                {
                                    inputuser = Console.ReadLine();
                                }
                                else if(command == "math")
                                {
                                    
                                    ints = await CSharpScript.EvaluateAsync<int>(arg);
                                    
                                }
                                else if (command == "hello")
                                {
                                    Console.WriteLine("Hello World!");
                                }

                                else if (command == "startfile")
                                {


                                    if (File.Exists(arg) == true)
                                    {
                                        if (arg.ToLower() != userfile.ToLower())
                                        {
                                            Process process = Process.Start(new ProcessStartInfo
                                            {
                                                FileName = arg,
                                                Arguments = arg2

                                            });
                                        }
                                        else { error = error + "\n" + $"в строке {linescount} допущена ошибка в слове {commands}:нельзя запутить файл"; }
                                    }
                                    else { error = error + "\n" + $"в строке {linescount} допущена ошибка в слове {commands}:файла не существует"; }

                                }
                                else if (command.Contains("cmd") == true)
                                {




                                    Process process = Process.Start(new ProcessStartInfo
                                    {
                                        FileName = "cmd",
                                        Arguments = "/c " + arg,
                                        UseShellExecute = false,
                                        RedirectStandardOutput = true,
                                        CreateNoWindow = true
                                    });



                                    string output = process.StandardOutput.ReadToEnd();
                                    Console.WriteLine(output);

                                }

                                else
                                {

                                    if (commands != "")
                                    {
                                        if (noterror == false)
                                        {
                                            error = error + "\n" + $"в строке {linescount} допущена ошибка в слове {commands}";

                                        }
                                        else { noterror = false; }
                                    }

                                }

                            }


                        }
                        linescount++;
                    }
                }

                else { error = error + "\n" + "Файл не существует или указан неправильный путь"; }
                if (error == "") { error = "0"; }

                Console.WriteLine($"Программа завершена. Код ошибки: {error}");

            }

        }
    }


}
