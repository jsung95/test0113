using EasyProject.Dao;
using EasyProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using System.Text.RegularExpressions;
using System.Windows;
using MaterialDesignThemes.Wpf;
using log4net;

namespace EasyProject.ViewModel
{
    public class SignupViewModel : Notifier
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        SignupDao dao = new SignupDao();

        public List<DeptModel> Depts { get; set; }    // Depts = DeptModel 객체가 담긴 리스트

        private DeptModel selectedDept; // 콤보박스에서 선택한 부서객체
        public DeptModel SelectedDept
        {
            get { return selectedDept; }
            set
            {
                selectedDept = value;
                OnPropertyChanged("SelectedDept");
            }
        }

        private bool isSignUp = false; // 콤보박스에서 선택한 부서객체
        public bool IsSignUp
        {
            get { return isSignUp; }
            set
            {
                isSignUp = value;
                OnPropertyChanged("IsSignUp");
            }
        }

        // 회원가입 시에 사용자가 입력한 데이터를 담을 프로퍼티       
        public NurseModel Nurse { get; set; }

        // 회원가입 시에 비밀번호 입력 값을 담을 프로퍼티
        private string nurse_Pw;
        public string Nurse_Pw
        {
            get { return nurse_Pw; }
            set
            {
                nurse_Pw = value;
                InsertPasswordCheck();
                OnPropertyChanged("Nurse_Pw");
            }
        }

        // 회원가입 시에 비밀번호 재입력 값을 담을 프로퍼티
        private string nurse_RePw;
        public string Nurse_RePw 
        {
            get { return nurse_RePw; }
            set
            {
                nurse_RePw = value;
                InsertPasswordCheck();
                OnPropertyChanged("Nurse_RePw");
            }
        }

        private void InsertPasswordCheck()
        {
            log.Info("InsertPasswordCheck() invoked.");
            try
            {
                if (Nurse_Pw.Equals(Nurse_RePw))
                {
                    SignUpMessage_PwRe = null;
                }
                else
                {
                    SignUpMessage_PwRe = "비밀번호가 일치하지 않습니다.";
                }
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//InsertPasswordCheck

        public SignupViewModel()
        {
            log.Info("Constructor SignupViewModel() invoked.");
            messagequeue = new SnackbarMessageQueue();

            Nurse = new NurseModel();
            Depts = dao.GetDeptModels();
        }//생성자

        //private ActionCommand command;
        //public ICommand Command
        //{
        //    get
        //    {
        //        if (command == null)
        //        {
        //            command = new ActionCommand(SignupInsert);
        //        }
        //        return command;
        //    }

        //}
        private ActionCommand resetCommand;
        public ICommand ResetCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new ActionCommand(ResetForm);
                }
                return resetCommand;
            }
        }


        //회원가입 스넥바
        private bool isSignUpOk = false;
        public bool IsSignUpOk 
        {
            get { return isSignUpOk; }
            set { isSignUpOk = value; OnPropertyChanged("IsSignUpOk"); }
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


        //회원가입 정규식 메시지
        private string signUpMessage_Dept;
        public string SignUpMessage_Dept
        {
            get { return signUpMessage_Dept; }
            set 
            {
                signUpMessage_Dept = value;
                OnPropertyChanged("SignUpMessage_Dept");
            }
        }

        private string signUpMessage_PwRe;
        public string SignUpMessage_PwRe
        {
            get { return signUpMessage_PwRe; }
            set
            {
                signUpMessage_PwRe = value;
                OnPropertyChanged("SignUpMessage_PwRe");
            }
        }



        // 회원가입 성공 유무를 체크하기 위한 프로퍼티
        //public bool isSignup { get; set; }

        public bool SignupInsert()
        {
            log.Info("SignupInsert() invoked.");
            try
            {
                bool SignupResult;

                if (Nurse.Nurse_name == null || Nurse.Nurse_no == null || Nurse_Pw == null || Nurse_RePw == null || SelectedDept == null)
                {
                    if(SelectedDept == null)
                    {
                        SignUpMessage_Dept = "담당 부서를 선택해주세요.";
                    }
                    IsSignUpOk = true;
                    MessageQueue.Enqueue("정보를 모두 입력해주세요.", "닫기", (x) => { IsSignUpOk = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                    SignupResult = false;
                    return SignupResult;
                }
                else
                {
                    SignUpMessage_Dept = null;
                    Regex regex = new Regex(@"^[0-9]{8}$"); //아이디는 숫자 8자리
                    if (regex.IsMatch(Convert.ToString(Nurse.Nurse_no)))
                    {
                        Nurse = dao.IdCheck(Nurse); //중복검사
                        Console.WriteLine("id reg ok");
                        regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$"); //비밀번호는 숫자,문자 조합
                        if (regex.IsMatch(Nurse_Pw))
                        {

                            if (Nurse.Nurse_no != null) // 중복없을시 가입 진행
                            {
                                if (Nurse_Pw.Equals(Nurse_RePw)) // 비밀번호, 비밀번호 재확인 값이 같다면 
                                {

                                    SignUpMessage_PwRe = null;

                                    Nurse.Nurse_pw = Nurse_Pw;

                                    dao.SignUp(Nurse, SelectedDept); //회원가입
                                                                     //MessageBox.Show("회원가입 완료!");
                                    IsSignUp = true;

                                    //회원가입을 1회 진행하면서 바인딩 되어서 남겨진 데이터 초기화
                                    Nurse.Nurse_no = null;
                                    Nurse.Nurse_name = null;
                                    SelectedDept = null;

                                    SignupResult = true;
                                    return SignupResult;
                                }//if
                                else // 입력한 두 비밀번호가 다르다면 
                                {
                                    SignUpMessage_PwRe = "비밀번호가 일치하지 않습니다.";

                                    IsSignUpOk = true;
                                    MessageQueue.Enqueue("입력한 두 비밀번호가 일치하지 않습니다.", "닫기", (x) => { IsSignUpOk = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                                    SignupResult = false;
                                    return SignupResult;
                                }//else                    
                            }//if
                            else // 중복 있으면 ( Nurse.Nurse_no가 dao.IdCheck(Nurse)에 의해 null 값임 )
                            {
                                IsSignUpOk = true;
                                MessageQueue.Enqueue("사용할 수 없는 아이디 입니다.", "닫기", (x) => { IsSignUpOk = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                                SignupResult = false;
                                return SignupResult;
                            }

                        }//if
                        else
                        {
                            IsSignUpOk = true;
                            MessageQueue.Enqueue("비밀번호는 숫자, 문자 조합만 6자리 이상만 가능합니다.", "닫기", (x) => { IsSignUpOk = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                            SignupResult = false;
                            return SignupResult;
                        }//else

                    }//if
                    else
                    {
                        IsSignUpOk = true;
                        MessageQueue.Enqueue("사번은 숫자 8자리만 입력가능합니다.", "닫기", (x) => { IsSignUpOk = true; }, null, false, true, TimeSpan.FromMilliseconds(3000));

                        SignupResult = false;
                        return SignupResult;
                    }//else

                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
            

        }//SignupResult

        public void ResetForm()
        {
            log.Info("ResetForm() invoked.");
            try
            {
                Nurse.Nurse_name = null;
                SelectedDept = null;
                Nurse.Nurse_no = null;
                Nurse_Pw = null;
                Nurse.Nurse_re_pw = null;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }
    } // SignupViewModel
} // namespace
