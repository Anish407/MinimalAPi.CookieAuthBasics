<h1>Cookie Authentication basics </h1>
<p>
  The projects in the solution contain samples that demonstrate cookie authentication. 
</p>
<ul>
    <li>
        <a href="https://github.com/Anish407/MinimalAPi.CookieAuthBasics/blob/master/CookieAuth.Api/Program.cs">CookieAuth.Api : 
        Contains code on how to configure cookie authentication and various options that can be used to configure the cookie. 
        </a>
    </li>
    <li>
     <a href="https://github.com/Anish407/MinimalAPi.CookieAuthBasics/blob/master/LearnAuthenticationSchemas.Api/Controllers/AuthController.cs">
      LearnAuthenticationSchemas.Api : Contains code on how to use authentication schemes, we have 3 endpoints. A user can access the  OnlyCustomers() if it has 
      the Customer cookie, similarly the OnlyLocal() can only be accessed using the local cookie, the anyone() endpoint can be accessed if either of 
      the cookies are present
     </a>
    </li>
    <li>
     <a href="https://github.com/Anish407/MinimalAPi.CookieAuthBasics/blob/master/MinimalAPi.AuthBasics/Controllers/AuthController.cs">
      MinimalAPi.AuthBasics : Contains code on how to use the dataprotection apis and how .net uses the dataprotection apis to encrypt and decrypt cookies
     . Shows how the Authentication and authorization middlewares work </a>
    </li>
     <li>
      <a href="https://github.com/Anish407/MinimalAPi.CookieAuthBasics/blob/master/MultipleAuthenticationSchemes.Api/Program.cs">
      MultipleAuthenticationSchemes.Api : Contains code on how to use Mutliple authentication schemes, We will have an endpoint that can only be accessed
      using a cookie or JWT tokens
      </a>
    </li>
   
</ul>
