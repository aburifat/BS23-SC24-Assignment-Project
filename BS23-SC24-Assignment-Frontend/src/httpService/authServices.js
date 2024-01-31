import { GET, POST } from "./service";

export const RegisterUserService = (request) => {
  return POST(`/api/register`, request);
};

export const LoginUserService = (request) => {
  return POST(`/api/login`, request);
};

export const TokenValidationService = () => {
  return GET(`/api/token-validity-check`);
};
