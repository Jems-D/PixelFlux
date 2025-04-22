import axios from "axios";
import { errorHandler } from "../Helpers/ErrorHandler";
import { PortfolioGet, PorfolioPost } from "../Models/Portfolio";

const api = "http://localhost:5005/api/portfolio/";

export const getAllPortfolioAPI = async () => {
  try {
    const data = await axios.get<PortfolioGet[]>(api);
    return data;
  } catch (err) {
    errorHandler(err);
  }
};

export const addPortfolioAPI = async (symbol: string) => {
  try {
    const data = await axios.post<PorfolioPost>(`${api}?Symbol=${symbol}`);
    return data;
  } catch (err) {
    errorHandler(err);
  }
};

export const deletePortfolioAPI = async (symbol: string) => {
  try {
    const data = await axios.delete<PorfolioPost>(`${api}?Symbol=${symbol}`);
    return data;
  } catch (err) {
    errorHandler(err);
  }
};
