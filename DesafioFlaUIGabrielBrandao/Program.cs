using System;
using FlaUI.Core;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;
using System.Diagnostics;

namespace DesafioFlaUIGabrielBrandao
{
    internal class Program
    {
        static void Main()
        {
            //Inicia a aplicação
            var app = Application.Launch(@"C:\Users\bielf\Desktop\UIDemo.exe"); //adicione seu diretório @"local do arquivo\UIDemo.exe"
            ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());


            //Acha o Identificador do Processo (Process ID) para uso posterior
            int idProcesso = 0;
            Process[] processList = Process.GetProcesses();

            for (int i = 0; i < processList.Length; i++)
            {
                if (processList[i].ProcessName == "UIDemo")
                {
                    idProcesso = processList[i].Id;
                }
            }


            //Define a janela que será automatizada e faz o Log In
            var logInWindow = app.GetMainWindow(new UIA3Automation());
            logInWindow.FindFirstChild(cf.ByAutomationId("user")).AsTextBox().Enter("admin");
            logInWindow.FindFirstChild(cf.ByAutomationId("pass")).AsTextBox().Enter("password");
            logInWindow.FindFirstChild(cf.ByName("Log In")).AsButton().Click();


            //Define a nova janela após o Log In e manipula os dados
            var mainWindow = app.GetMainWindow(new UIA3Automation());
            mainWindow.FindFirstChild(cf.ByName("Use Cash Count")).AsCheckBox().Click();
            mainWindow.FindFirstChild(cf.ByName("Use Amount")).AsRadioButton().Click();
            mainWindow.FindFirstChild(cf.ByName("ChangeTItle")).AsCheckBox().Click();


            //Define a janela após a alteração do título e manipula outros dados
            var mainWindow2 = app.GetMainWindow(new UIA3Automation());


            //Acha o título da janela de aplicação usando o Process ID encontrado anteriormente
            String nomeProcesso = null;
            for (int i = 0; i < processList.Length; i++)
            {
                if (processList[i].Id == idProcesso)
                {
                    nomeProcesso = processList[i].MainWindowTitle;
                }
            }
            Console.WriteLine(nomeProcesso);


            //Retira a data do título da janela e coloca na formatação pedida
            String data = nomeProcesso.Substring(7, 10);
            DateTime dataTodate = Convert.ToDateTime(data);
            Console.WriteLine(dataTodate.ToString("yyyy/MM/d"));


            //Continua manipulando os dados da janela
            mainWindow2.FindFirstDescendant(cf.ByAutomationId("cashintb")).AsTextBox().Enter("40");
            mainWindow2.FindFirstDescendant(cf.ByAutomationId("notonustb")).AsTextBox().Enter("100");
            mainWindow2.FindFirstDescendant(cf.ByName("Accept")).AsButton().Click();


            //Decide se fecha, ou não, a aplicação
            Console.WriteLine("Deseja fechar a aplicação? Digite 'sim' se quiser");
            String resposta = Console.ReadLine();
            if (resposta == "sim")
            {
                mainWindow2.FindFirstDescendant(cf.ByName("Exit")).AsButton().Click();
            }
            else
            {
                Console.WriteLine("Resposta diferente, mantendo janela aberta");
            }
        }
    }
}
