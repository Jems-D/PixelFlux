import { UserProfile } from "../Models/User";
import { useNavigate } from "react-router-dom";
import { createContext, useEffect, useState } from "react";
import { loginApi, registerApi } from "../Services/AuthService";
import { toast } from "react-toastify";
import React from "react";
import axios from "axios";

type userContextType = {
  user: UserProfile | null;
  token: string | null;
  registerUser: (email: string, username: string, password: string) => void;
  loginUser: (username: string, password: string) => void;
  logoutUser: () => void;
  isLoggedIn: () => boolean;
};
type Props = { children: React.ReactNode };

const UserContext = createContext<userContextType>({} as userContextType);

export const UserProvider = ({ children }: Props) => {
  const navigate = useNavigate();
  const [token, setToken] = useState<string | null>(null);
  const [user, setUser] = useState<UserProfile | null>(null);
  const [isReady, setIsReady] = useState<boolean>(false);

  useEffect(() => {
    const user = localStorage.getItem("user");
    const token = localStorage.getItem("token");

    if (user && token) {
      setUser(JSON.parse(user));
      setToken(token);
      axios.defaults.headers.common["Authorization"] = "Bearer " + token;
    }
    setIsReady(true);
  }, []);

  const registerUser = async (
    username: string,
    email: string,
    password: string
  ) => {
    await registerApi(username, email, password)
      .then((res) => {
        if (res) {
          localStorage.setItem("token", res?.data.token);
          const userObj = {
            userName: res?.data.username,
            email: res?.data.email,
          };
          localStorage.setItem("user", JSON.stringify(userObj));
          setToken(res?.data.token);
          setUser(userObj);
          toast.success("Login Success");
          navigate("/search");
        }
      })
      .catch((e) => toast.error("Server Error", e));
  };

  const loginUser = async (username: string, password: string) => {
    await loginApi(username, password)
      .then((res) => {
        if (res) {
          localStorage.setItem("token", res?.data.token);
          const userObj = {
            userName: res?.data.username,
            email: res?.data.email,
          };
          localStorage.setItem("user", JSON.stringify(userObj));
          setToken(res?.data.token);
          setUser(userObj);
          toast.success("Login Success");
          navigate("/search");
        }
      })
      .catch((e) => toast.error("Server Error"));
  };

  const isLoggedIn = () => {
    return !!user;
  };

  const logoutUser = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setUser(null);
    setToken("");
    navigate("/");
  };

  return (
    <UserContext.Provider
      value={{ loginUser, user, token, logoutUser, isLoggedIn, registerUser }}
    >
      {isReady ? children : null}
    </UserContext.Provider>
  );
};

export const useAuth = () => React.useContext(UserContext);
