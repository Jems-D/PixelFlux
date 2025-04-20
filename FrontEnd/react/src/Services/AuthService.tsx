import axios from "axios";
import { errorHandler } from "../Helpers/ErrorHandler";
import { UserProfileToken } from "../Models/User";

const api = "http://localhost:5005/api/";

export const loginApi = async (username: string, password: string) => {
  try {
    const data = await axios.post<UserProfileToken>(api + "account/login", {
      username: username,
      password: password,
    });
    return data;
  } catch (err) {
    errorHandler(err);
  }
};

export const registerApi = async (
  username: string,
  email: string,
  password: string
) => {
  try {
    const data = await axios.post<UserProfileToken>(api + "account/register", {
      username: username,
      email: email,
      password: password,
    });
    return data;
  } catch (err) {
    errorHandler(err);
  }
};
