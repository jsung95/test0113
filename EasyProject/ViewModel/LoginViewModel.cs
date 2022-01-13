using EasyProject.Model;
using EasyProject.Dao;
using System;
using Microsoft.Expression.Interactivity.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using log4net;
using MaterialDesignThemes.Wpf;

namespace EasyProject.ViewModel
{
    public class LoginViewModel : Notifier
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        LoginDao dao = new LoginDao();

        private NurseModel nurse;
        public NurseModel Nurse
        {
            get { return nurse; }
            set 
            { 
                nurse = value;
                //OnPropertyChanged("Nurse");
            }
        }

        public INavigation Navigation { get; set; }


        //스넥바 메세지큐
        private SnackbarMessageQueue messagequeue;
        public SnackbarMessageQueue MessageQueue
        {
            get { return messagequeue; }
            set
            {
                messagequeue = value;
                OnPropertyChanged("MessageQueue");
            }
        }

        private bool isLoginOk = false;
        public bool IsLoginOk
        {
            get { return isLoginOk; }
            set { isLoginOk = value; OnPropertyChanged("IsLoginOk"); }
        }


        public LoginViewModel()
        {
            log.Info("Constructor LoginViewModel() invoked.");
            messagequeue = new SnackbarMessageQueue();
            Nurse = new NurseModel();
        }


        //private ActionCommand command;
        //public ICommand Command
        //{
        //    get
        //    {
        //        if (command == null)
        //        {
        //            command = new ActionCommand(Login);
        //        }
        //        return command;
        //    }
        //}

        
        public bool idCheckResult { get; set; }


        private ActionCommand logout;
        public ICommand LogoutCommand
        {
            get
            {
                if(logout == null)
                {
                    logout = new ActionCommand(Logout);
                }
                return logout;
            }
        }
        public bool Login()
        {
            log.Info("Login() invoked.");

            try
            {
                idCheckResult = dao.IdPasswordCheck(Nurse); // id/pw 가 일치하는 지 확인
                Console.WriteLine("Login() idCheckResult: " + idCheckResult);
                if (idCheckResult == true) // 일치할 경우
                {
                    Console.WriteLine("id password check ok!");
                    NurseModel result = dao.LoginUserInfo(Nurse); // 해당 사용자 정보를 NurseModel 객체에 넣는다.   

                    dao.Login_Logging(result); //로깅 데이터도 추가

                    App.nurse_dto.Nurse_no = result.Nurse_no;
                    App.nurse_dto.Nurse_name = result.Nurse_name;
                    App.nurse_dto.Nurse_auth = result.Nurse_auth;
                    App.nurse_dto.Nurse_pw = result.Nurse_pw;
                    App.nurse_dto.Dept_id = result.Dept_id;

                    log4net.GlobalContext.Properties["user_no"] = App.nurse_dto.Nurse_no;
                    log4net.GlobalContext.Properties["user_name"] = App.nurse_dto.Nurse_name;
                    log4net.GlobalContext.Properties["user_auth"] = App.nurse_dto.Nurse_auth;
                    log.Info("Login Sucess");
                    Console.WriteLine("로그인 성공");
                    Console.WriteLine("  Nurse NO : {0}", App.nurse_dto.Nurse_no);
                    Console.WriteLine("  Nurse NAME : {0}", App.nurse_dto.Nurse_name);
                    Console.WriteLine("  Nurse AUTH : {0}", App.nurse_dto.Nurse_auth);
                    Console.WriteLine("  Nurse PW : {0}", App.nurse_dto.Nurse_pw);
                    Console.WriteLine("  DEPT ID : {0}", App.nurse_dto.Dept_id);

                    return idCheckResult;

                }//if
                else
                {
                    Console.WriteLine("id password check fail!");
                    Console.WriteLine("로그인 실패");
                    Console.WriteLine("  Nurse NO : {0}", App.nurse_dto.Nurse_no);
                    Console.WriteLine("  Nurse NAME : {0}", App.nurse_dto.Nurse_name);
                    Console.WriteLine("  Nurse AUTH : {0}", App.nurse_dto.Nurse_auth);
                    Console.WriteLine("  Nurse PW : {0}", App.nurse_dto.Nurse_pw);
                    Console.WriteLine("  DEPT ID : {0}", App.nurse_dto.Dept_id);

                    return idCheckResult;
                }//else
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return idCheckResult;
            }//catch
        }//Login

        public void Logout()
        {
            log.Info("Logout() invoked.");

            try
            {
                dao.Logout_Logging(App.nurse_dto); //로깅 데이터도 추가

                App.nurse_dto.Nurse_no = null;
                App.nurse_dto.Nurse_name = null;
                App.nurse_dto.Nurse_auth = null;
                App.nurse_dto.Nurse_pw = null;
                App.nurse_dto.Dept_id = null;

                log4net.GlobalContext.Properties["user_no"] = App.nurse_dto.Nurse_no;
                log4net.GlobalContext.Properties["user_name"] = App.nurse_dto.Nurse_name;
                log4net.GlobalContext.Properties["user_auth"] = App.nurse_dto.Nurse_auth;

                Console.WriteLine("로그아웃 성공");
                Console.WriteLine("  Nurse NO : {0}", App.nurse_dto.Nurse_no);
                Console.WriteLine("  Nurse NAME : {0}", App.nurse_dto.Nurse_name);
                Console.WriteLine("  Nurse AUTH : {0}", App.nurse_dto.Nurse_auth);
                Console.WriteLine("  Nurse PW : {0}", App.nurse_dto.Nurse_pw);
                Console.WriteLine("  DEPT ID : {0}", App.nurse_dto.Dept_id);

                //로그아웃 버튼 클릭 시에 프로그램 재시작
                System.Windows.Forms.Application.Restart();
                System.Windows.Application.Current.Shutdown();
            }//try
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//logout

        public void Logout_For_QuitProgram()
        {
            log.Info("Logout_For_QuitProgram");
            try
            {
                dao.Logout_Logging(App.nurse_dto); //로깅 데이터도 추가
            }//try
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//Logout_For_QuitProgram


        private string password;

        public string Password 
        { 
            get => password;
            set
            { 
                password = value;
                nurse.Nurse_pw = value;
            }
         }

    }//class

}//namespace
