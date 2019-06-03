# Identity no ASP.NET Core

O ASP.NET Core Identity é um sistema de usuários que adiciona funcionalide de login a um sistema ASP.NET Core. Usuários podem criar uma conta com as informações de login guardadas no Identity ou utitlizar um provedor externo de credenciais, podendo por exemplo ser: [Facebook, Google, Microsoft Account, e Twitter](https://docs.microsoft.com/pt-br/aspnet/core/security/authentication/social/index?view=aspnetcore-3.0).

O Identity pode ser configurado, por exemplo, um banco de dados SQL. Mas também pode funcionar utilizando outros recursos, como o Azure Table Storage. Neste laboratório, como configurado por padrão ao gerar um WebApp com a CLI do `dotnet`, o banco de dados utilizado é um SQLite, criado automaticamente na pasta raiz do projeto, sobre o nome de `app.db`.

Durante o laboratório, você irá conhecer como utilizar o Identity para registrar, logar e deslogar um usuário.



> #### AddDefaultIdentity e AddIdentity
>
> Na versão do ASP.NET Core 2.1 foi adicionada a referência [AddDefaultIdentity](https://docs.microsoft.com/pt-br/dotnet/api/microsoft.extensions.dependencyinjection.identityservicecollectionuiextensions.adddefaultidentity?view=aspnetcore-2.1#Microsoft_Extensions_DependencyInjection_IdentityServiceCollectionUIExtensions_AddDefaultIdentity__1_Microsoft_Extensions_DependencyInjection_IServiceCollection_System_Action_Microsoft_AspNetCore_Identity_IdentityOptions__) Ao adicionar `AddDefaultIdentity`, você estará adicionando automaticamente as referências:
>
> - [AddIdentity](https://docs.microsoft.com/pt-br/dotnet/api/microsoft.extensions.dependencyinjection.identityservicecollectionextensions.addidentity?view=aspnetcore-2.1#Microsoft_Extensions_DependencyInjection_IdentityServiceCollectionExtensions_AddIdentity__2_Microsoft_Extensions_DependencyInjection_IServiceCollection_System_Action_Microsoft_AspNetCore_Identity_IdentityOptions__)
> - [AddDefaultUI](https://docs.microsoft.com/pt-br/dotnet/api/microsoft.aspnetcore.identity.identitybuilderuiextensions.adddefaultui?view=aspnetcore-2.1#Microsoft_AspNetCore_Identity_IdentityBuilderUIExtensions_AddDefaultUI_Microsoft_AspNetCore_Identity_IdentityBuilder_)
> - [AddDefaultTokenProviders](https://docs.microsoft.com/pt-br/dotnet/api/microsoft.aspnetcore.identity.identitybuilderextensions.adddefaulttokenproviders?view=aspnetcore-2.1#Microsoft_AspNetCore_Identity_IdentityBuilderExtensions_AddDefaultTokenProviders_Microsoft_AspNetCore_Identity_IdentityBuilder_)
>
> Veja [AddDefaultIdentity source](https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/UI/src/IdentityServiceCollectionUIExtensions.cs#L47-L63) para mais informações.



## Criando uma Web app com autenticação

Vamos começar criando um projeto ASP.NET Core Web Application com contas de Usuário Individuais

```cli
dotnet new webapp --auth Individual -o lab_identity
```

O projeto gerado contém o [ASP.NET Core Identity](https://docs.microsoft.com/pt-br/aspnet/core/security/authentication/identity?view=aspnetcore-3.0) e também a biblioteca [Razor Class](https://docs.microsoft.com/pt-br/aspnet/core/razor-pages/ui-class?view=aspnetcore-3.0). A biblioteca Identity Razor Class expõe os endpoints relacionados ao  `Identity`. Por exemplo:

- /Identity/Account/Login

- /Identity/Account/Logout

- /Identity/Account/Manage

  

### Realizando Migrações

Como estamos utilizando um banco de dados SQL e não mais o EF In-Memory, é interessante que preparemos o banco para receber o dados que iremos inserir. Como visto no início, por padrão o projeto é configurado para armazenar os dados da aplicação em um arquivo chamado app.db, na raiz do projeto. Portanto, não precisamos subir ou conectar outro servidor de banco de dados para prosseguir no laboratório.

```cli
dotnet ef database update
```



### Testando as funções de Registrar e Login

Sim, já está na hora de testar as funções principais do Identity. Como o banco de dados já está montado, é possível executar a aplicação através do comando:

```
dotnet run
```

e acessar no navegador o endereço, que se for o padrão, será https://localhost:5001. 

Ao acessar o link acima, é possível ver no canto superior da página as opções de Registrar e Logar. Como o banco de dados está limpo, é necessário que você se registre para conseguir logar com sucesso. Por padrão, ao criar um usuário novo, sua senha deve ter pelo menos:

- Uma letra minúscula
- Uma letra maiúscula
- Um dígito
- Um caracter não-alfanumérico (!@#%, etc)
- 6 caracteres de comprimento

Criada a conta, basta acessar a página de login e logar. Caso tudo tenha ocorrido com sucesso, você deve na barra de navegação os dizeres: "Hello <seu email>!".

A rapidez em ter um sistema de login funcionando em poucos minutos demonstra a facilidade de se criar uma aplicação utilizando os parâmetros pré-definidos do comando `dotnet new`. Para ver todos os comandos disponíveis, basta escrever `dotnet new —help`.

### Configurando os serviços do Identity

Os serviços são adicionados em `ConfigureServices`. É comum chamar totdos os métodos `Add{Service}`, e só depois os métodos de configuração de serviço - `services.Configure{Service}`.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.Configure<CookiePolicyOptions>(options =>
    {
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
    });

    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(
            Configuration.GetConnectionString("DefaultConnection")));
    services.AddDefaultIdentity<IdentityUser>()
        .AddDefaultUI(UIFramework.Bootstrap4)
        .AddEntityFrameworkStores<ApplicationDbContext>();

    services.Configure<IdentityOptions>(options =>
    {
        // Configurações de senha, para deixar mais ou menos complexa.
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;

        // Configurações de travamento de contat.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // Configurações de nome/email de usuário
        options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = false;
    });

    services.ConfigureApplicationCookie(options =>
    {
        // Configurações de Cookies
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

        options.LoginPath = "/Identity/Account/Login";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.SlidingExpiration = true;
    });

    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
}
```

O código acima configura o Identity com as opções padrão. Fique à vontade para adicionar ou remover restrições. 

Por final, o Identity é habilitado na aplicação ao chamar o método [UseAuthentication](https://docs.microsoft.com/pt-br/dotnet/api/microsoft.aspnetcore.builder.authappbuilderextensions.useauthentication#Microsoft_AspNetCore_Builder_AuthAppBuilderExtensions_UseAuthentication_Microsoft_AspNetCore_Builder_IApplicationBuilder_). `app.UseAuthentication()` adiciona o Middleware de autenticação às pipelines de requests. Dessa forma, é possível adicionar uma restrição a uma rota apenas incluindo `[Authorize]` antes de sua definição, como veremos posteriormente.

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseCookiePolicy();

    app.UseAuthentication();

    app.UseMvc();
}
```

Para mais informações, veja [IdentityOptions Class](https://docs.microsoft.com/pt-br/dotnet/api/microsoft.aspnetcore.identity.identityoptions) e [Application Startup](https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/startup?view=aspnetcore-3.0).



### Examinando o fluxo de Registrar

Quando um usuário clica no link de Registrar, a função `RegisterModel.OnPostAsync` é chamada. Então, o usuário é criado pelo método [CreateAsync](https://docs.microsoft.com/pt-br/dotnet/api/microsoft.aspnetcore.identity.usermanager-1.createasync#Microsoft_AspNetCore_Identity_UserManager_1_CreateAsync__0_System_String_) do objeto `_userManager` . O objeto `_userManager` é disponibilizado pela injeção de dependências:

```csharp
public async Task<IActionResult> OnPostAsync(string returnUrl = null)
{
    returnUrl = returnUrl ?? Url.Content("~/");
    if (ModelState.IsValid)
    {
        var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
        var result = await _userManager.CreateAsync(user, Input.Password);
        if (result.Succeeded)
        {
            _logger.LogInformation("User created a new account with password.");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = user.Id, code = code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    // If we got this far, something failed, redisplay form
    return Page();
}
```

Se tudo ocorrer como devido, o usuário é logado através da chamada à função `_signInManager.SignInAsync`.

**Nota:** Veja [account confirmation](https://docs.microsoft.com/pt-br/aspnet/core/security/authentication/accconfirm?view=aspnetcore-3.0#prevent-login-at-registration) para previnir que o usuário logue imediatamente após o registro.



### Examinando o fluxo de Log in

Por padrão, o formulário de login é mostrado quando:

- A página de login é acessada.
- Um usuário tenta acessar uma página restrita a qual ele não é autorizado ou não está autenticado.

Quando o formulário da página de login é enviado, o método `OnPostAsync` é chamado. A função `PasswordSignInAsync` do objeto `_signInManager` é chamada (disponibilizado pela injeção de dependências).

```csharp
public async Task<IActionResult> OnPostAsync(string returnUrl = null)
{
    returnUrl = returnUrl ?? Url.Content("~/");

    if (ModelState.IsValid)
    {
        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, 
        // set lockoutOnFailure: true
        var result = await _signInManager.PasswordSignInAsync(Input.Email, 
            Input.Password, Input.RememberMe, lockoutOnFailure: true);
        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in.");
            return LocalRedirect(returnUrl);
        }
        if (result.RequiresTwoFactor)
        {
            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
        }
        if (result.IsLockedOut)
        {
            _logger.LogWarning("User account locked out.");
            return RedirectToPage("./Lockout");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }

    // If we got this far, something failed, redisplay form
    return Page();
}
```

O `Controller` base expõe uma propriedade `User `que pode ser acessada pelos métodos do Controller. Por exemplo, você pode enumerar Claims do usuário através de `User.Claims` e realizar decisões de autorização. Para mais informações, veja [Introdução à autorização no ASP.NET Core](https://docs.microsoft.com/pt-br/aspnet/core/security/authorization/introduction?view=aspnetcore-3.0).



### Examinando o fluxo de Log out

O link de logou chama o método `LogoutModel.OnPost`.

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace lab_identity.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return Page();
            }
        }
    }
}
```

O método [SignOutAsync](https://docs.microsoft.com/pt-br/dotnet/api/microsoft.aspnetcore.identity.signinmanager-1.signoutasync#Microsoft_AspNetCore_Identity_SignInManager_1_SignOutAsync) limpa os dados do usuário guardados nos cookies. Caso você redirecione o usuário depois de chamar o método, ele não será deslogado.

O método Post é especificado em *Pages/Shared/_LoginPartial.cshtml*:

```csharp
@using Microsoft.AspNetCore.Identity
@inject SignInManager<IdentityUser> SignInManager
@inject UserManager<IdentityUser> UserManager

<ul class="navbar-nav">
    @if (SignInManager.IsSignedIn(User))
    {
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="Identity"
               asp-page="/Account/Manage/Index"
               title="Manage">Hello@User.Identity.Name!</a>
        </li>
        <li class="nav-item">
            <form class="form-inline" asp-area="Identity" asp-page="/Account/Logout" 
                   asp-route-returnUrl="@Url.Page("/", new { area = "" })" 
                   method="post">
                <button type="submit" class="nav-link btn btn-link text-dark">Logout</button>
            </form>
        </li>
    }
    else
    {
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="Identity" asp-page="/Account/Register">Register</a>
        </li>
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="Identity" asp-page="/Account/Login">Login</a>
        </li>
    }
</ul>
```



## Testando o Identity

Por padrão, o template criado permite acesso anônimo (sem logar) às páginas iniciais. Para testar o Identity, basta adicionar [`[Authorize]`](https://docs.microsoft.com/pt-br/dotnet/api/microsoft.aspnetcore.authorization.authorizeattribute)  ao modelo da página de Privacy. `Pages/Privacy.cshtml.cs`

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lab_identity.Pages
{
    [Authorize]
    public class PrivacyModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
```

Se estiver logado, faça logout. Execute a aplicação novamente e tente clicar no link de **Privacy**. Você será redirecionado para a página de Login automaticamente.



> #### Componentes Identity
>
> Todos os pacotes do Identyty estão incluídos no pacote [Microsoft.AspNetCore.App metapackage](https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/metapackage-app?view=aspnetcore-3.0). O pacote primário é [Microsoft.AspNetCore.Identity](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity/). Ele contém as interfaces mais importantes do  ASP.NET Core Identity, e é incluído por `Microsoft.AspNetCore.Identity.EntityFrameworkCore`.

