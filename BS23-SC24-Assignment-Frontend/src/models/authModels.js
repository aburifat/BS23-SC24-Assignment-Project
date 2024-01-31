export const UserRoles = {
  Administrator: 1,
  Regular: 2,
};

export class RegisterUserModel {
  userName;
  email;
  password;
  confirmPassword;
}

export class LoginUserModel {
  userName;
  password;
}

export class GetUserModel {
  userId;
  userName;
  email;
  userRole;
}
