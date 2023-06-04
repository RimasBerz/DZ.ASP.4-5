using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using WebApplication1.Data;
using WebApplication1.Models.User;
using WebApplication1.Servicies.Hash;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IHashService _hashService;
        public UserController(DataContext dataContext, IHashService hashService)
        {
            _dataContext = dataContext;
            _hashService = hashService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp(SignUpFormModel? formModel)
        {
            SignUpViewModel viewModel = new();
            if (Request.Method == "POST" && formModel != null)
            {
                viewModel = ValidateSignUpForm(formModel);
                viewModel.FormModel = formModel;

                HttpContext.Session.SetString("FormData",
                    System.Text.Json.JsonSerializer.Serialize(viewModel)); 
                return RedirectToAction(nameof(SignUp));
            }
            else
            {
                if (HttpContext.Session.Keys.Contains("FormData"))
                {
                    String? data = HttpContext.Session.GetString("FormData");
                    if(data != null)
                    {
                        viewModel = System.Text.Json.
                        JsonSerializer.Deserialize<SignUpViewModel>(data)!;
                    }
                    HttpContext.Session.Remove("FormData");
                }
                else
                {
                    viewModel = new();
                    viewModel.FormModel = null; // нечего проверять
                }
            }
            return View(viewModel);
        }

        private SignUpViewModel ValidateSignUpForm(SignUpFormModel formModel)
        {
            SignUpViewModel viewModel = new();

            #region Email Validation
            if (string.IsNullOrEmpty(formModel.Email))
            {
                viewModel.EmailMessage = "Логин не может быть пустым";
            }
            else if (formModel.Email.Length < 3)
            {
                viewModel.EmailMessage = "Логин слишком короткий (минимум 3 символа)";
            }
            else if (_dataContext.Users.Any(u => u.Email == formModel.Email))
            {
                viewModel.EmailMessage = "Данный логин уже занят";
            }
            #endregion

            #region Password Validation
            if (string.IsNullOrEmpty(formModel.Password))
            {
                viewModel.PasswordMessage = "Пароль не может быть пустым";
            }
            else if (formModel.Password.Length < 3)
            {
                viewModel.PasswordMessage = "Пароль слишком короткий (минимум 3 символа)";
            }
            else if (!Regex.IsMatch(formModel.Password, @"\d"))
            {
                viewModel.PasswordMessage = "Пароль должен содержать цифры";
            }
            else
            {
                viewModel.PasswordMessage = null;
            }
            #endregion

            #region Email Validation
            if (string.IsNullOrEmpty(formModel.Email))
            {
                viewModel.EmailMessage = "Email не может быть пустым";
            }
            else if (formModel.Email.Length < 7)
            {
                viewModel.EmailMessage = "Некорректный формат email";
            }
            else
            {
                viewModel.EmailMessage = null;
            }
            #endregion

            #region Real Name Validation
            if (string.IsNullOrEmpty(formModel.RealName))
            {
                viewModel.RealNameMessage = "Имя не может быть пустым";
            }
            else
            {
                viewModel.RealNameMessage = null;
            }
            #endregion

            #region Avatar Validation
            String nameAvatar = null!;
            if (formModel.Avatar != null)
            {
                if (formModel.Avatar.Length > 1048576)
                {
                    viewModel.AvatarMessage = "Файл слишком большой (макс 1 МБ)";
                }
                String ext = Path.GetExtension(formModel.Avatar.FileName);

                nameAvatar = Guid.NewGuid().ToString() + ext;

                formModel.Avatar.CopyTo(new FileStream("wwwroot/avatars/" + nameAvatar, FileMode.Create));
            }
            #endregion


            if (viewModel.EmailMessage == null && 
                viewModel.PasswordMessage == null &&
                viewModel.AvatarMessage == null)
            {
                _dataContext.Users.Add(new()
                {
                    Id = Guid.NewGuid(),
                    Email = formModel.Email,
                    PasswordHash = _hashService.GetHash(formModel.Password),
                    CreatedDt = DateTime.Now,
                    Name = formModel.RealName!,
                    Avatar = nameAvatar
                });
                _dataContext.SaveChanges();


                viewModel.SuccessMessage = "Регистрация прошла успешно";
                formModel.Login = string.Empty;
                formModel.Password = string.Empty;
                formModel.Email = string.Empty;
                formModel.RealName = string.Empty;
                formModel.Avatar = null;
                viewModel.LoginMessage = null;
                viewModel.PasswordMessage = null;
                viewModel.EmailMessage = null;
                viewModel.RealNameMessage = null;
                viewModel.AvatarMessage = null;
            }

            return viewModel;
        }
    }
}
