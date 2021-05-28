namespace CloudDataProtection.Services.MailService.Business.Base
{
    public abstract class MailLogicBase
    {
        protected static readonly string Template = @"
<div style='width: max(40%, 480px); font-family:sans-serif;'>
  <div style='background-color: #3f51b5; display: block; font-family: sans-serif; color: #fff; padding: 1rem; box-shadow: 0px 2px 4px -1px rgba(0,0,0,0.2),0px 4px 5px 0px rgba(0,0,0,0.14),0px 1px 10px 0px rgba(0,0,0,0.12)'>Cloud Data Protection</div>
  {0}
</div>";

        protected string ComposeBody(string content) => string.Format(Template, content);
    }
}