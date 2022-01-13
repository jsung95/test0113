using EasyProject.Dao;
using EasyProject.Model;
using log4net;
using MaterialDesignThemes.Wpf;
using Microsoft.Expression.Interactivity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace EasyProject.ViewModel
{
    public class PasswordChangeViewModel : Notifier
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        LoginDao dao = new LoginDao();

        public string Nurse_no { get; set; }
        public string Nurse_pw { get; set; }
        public NurseModel Nurse { get; set; }

        private string newPassword;
        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                newPassword = value;
                OnPropertyChanged("NewPassword");
                OnNewPasswordChanged();
            }
        }
        private string re_NewPassword;
        public string Re_NewPassword
        {
            get { return re_NewPassword; }
            set
            {
                re_NewPassword = value;
                OnPropertyChanged("Re_NewPassword");
                OnNewPasswordChanged();
            }
        }
        private string newPasswordStatement;

        public string NewPasswordStatement
        {
            get { return newPasswordStatement; }
            set
            {
                newPasswordStatement = value;
                OnPropertyChanged("NewPasswordStatement");
            }
        }


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

        private bool isPwChangOk = false;
        public bool IsPwChangOk
        {
            get { return isPwChangOk; }
            set
            {
                isPwChangOk = value;
                OnPropertyChanged("IsPwChangOk");
            }
        }


        public PasswordChangeViewModel()
        {
            log.Info("Constructor PasswordChangeViewModel() invoked.");
            messagequeue = new SnackbarMessageQueue();
            Nurse = new NurseModel();
            NewPassword = "";


        }

        //public ActionCommand command;

        //public ICommand Command
        //{
        //    get
        //    {
        //        if (command == null)
        //        {
        //            command = new ActionCommand(PasswordChange);
        //        }
        //        return command;
        //    }
        //}

        public bool pwChangeResult { get; set; }

        public bool PasswordChange()
        {
            log.Info("Constructor PasswordChange() invoked.");

            //bool pwChangeResult;

            try
            {
                if (dao.IdPasswordCheck(Nurse) == true) // 현재 아이디/비번이 맞는 지 확인
                {
                    // 비밀번호 변경시 새 비밀번호 공백 입력 방지
                    if (NewPassword == "" || NewPassword == null)
                    {
                        IsPwChangOk = true;
                        MessageQueue.Enqueue("새로운 비밀번호를 입력하세요!", "닫기", (x) => { IsPwChangOk = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                        pwChangeResult = false;
                        return pwChangeResult;
                    }
                    else if (Re_NewPassword == "" || Re_NewPassword == null)
                    {
                        IsPwChangOk = true;
                        MessageQueue.Enqueue("다시 입력란을 채워주세요!", "닫기", (x) => { IsPwChangOk = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                        pwChangeResult = false;
                        return pwChangeResult;
                    }
                    else if (NewPassword == Nurse.Nurse_pw)
                    {
                        IsPwChangOk = true;
                        MessageQueue.Enqueue("현재 비밀번호와 다른 비밀번호를 입력해주세요!", "닫기", (x) => { IsPwChangOk = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                        pwChangeResult = false;
                        return pwChangeResult;
                    }
                    // 새 비밀번호와 다시입력 같은지 확인
                    else if (NewPassword == Re_NewPassword)
                    {
                        Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$"); //비밀번호는 숫자,문자 조합
                        if (regex.IsMatch(NewPassword))
                        {
                            IsPwChangOk = false;
                            MessageQueue.Enqueue("비밀번호 변경 완료!", "닫기", (x) => { IsPwChangOk = false; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                            dao.PasswordChange(Nurse, NewPassword);
                            //비밀번호 변경을 1회 진행하면서 바인딩 되어서 남겨진 데이터 초기화
                            Nurse.Nurse_no = null;
                            Nurse.Nurse_pw = null;

                            pwChangeResult = true;
                            return pwChangeResult;
                        }
                        else
                        {
                            IsPwChangOk = true;
                            MessageQueue.Enqueue("비밀번호는 숫자, 문자 조합만 6자리 이상만 가능합니다.", "닫기", (x) => { IsPwChangOk = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                            pwChangeResult = false;
                            return pwChangeResult;
                        }
                    }//else if
                    else
                    {
                        IsPwChangOk = true;
                        MessageQueue.Enqueue("새 비밀번호가 일치하지 않습니다.", "닫기", (x) => { IsPwChangOk = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                        pwChangeResult = false;
                        return pwChangeResult;
                    }
                }//if
                else
                {
                    IsPwChangOk = true;
                    MessageQueue.Enqueue("아이디 또는 비밀번호를 다시 확인해주세요.", "닫기", (x) => { IsPwChangOk = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                    pwChangeResult = false;
                    return pwChangeResult;
                }//else
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return pwChangeResult;
            }//catch
            
        }//PasswordChange

        public void OnNewPasswordChanged()
        {
            log.Info("OnNewPasswordChanged() invoked.");

            try
            {
                //if (NewPassword == "" || Re_NewPassword == "")
                //{
                //    NewPasswordStatement = "";
                //}
                Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$"); //비밀번호는 숫자,문자 조합 6자리

                if (regex.IsMatch(NewPassword)) //정규식 통과 시 * 정규식을 통과 = NewPassword는 공란이 아님.
                {
                    if (Re_NewPassword == "" || Re_NewPassword == null)
                    {
                        NewPasswordStatement = "새 비밀번호를 한번 더 입력하세요!";
                    }
                    else
                    {
                        if (NewPassword == Re_NewPassword)
                        {
                            NewPasswordStatement = "두 비밀번호가 일치합니다.";
                        }
                        else
                        {
                            NewPasswordStatement = "두 비밀번호가 일치하지 않습니다.";
                        }
                    }

                }//if 
                else
                {
                    NewPasswordStatement = "비밀번호는 숫자,문자 조합 6자리 이상입니다";
                }//else            
            }//try
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }//catch
            
        }//OnPasswordChange
        private ActionCommand formResetCommand;
        public ICommand FormResetCommand
        {
            get
            {
                if (formResetCommand == null)
                {
                    formResetCommand = new ActionCommand(FormReset);
                }
                return formResetCommand;
            }//get
        }

        private void FormReset()
        {
            
            Nurse.Nurse_pw = "";
            NewPassword = "";
            Re_NewPassword = "";
            
        }
    }//class
}//namespace
