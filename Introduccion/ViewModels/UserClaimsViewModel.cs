using System.Collections.Generic;

namespace Introduccion.ViewModels
{
  public class UserClaimsViewModel
  {
    public string IdUser { get; set; }
    public List<UserClaims> Claims { get; set; }

    public UserClaimsViewModel()
    {
      Claims = new List<UserClaims>();
    }
  }
}
