import axios from "axios";
import {
  CompanyBalanceSheet,
  CompanyCashFlow,
  CompanyCompData,
  CompanyIncomeStatement,
  CompanyKeyMetrics,
  CompanyProfile,
  CompanySearch,
  tenKData,
} from "./company";
const apiKey = import.meta.env.VITE_API_KEY;

interface SearchResponse {
  data: CompanySearch[];
}

export const searchCompanies = async (query: string) => {
  try {
    const data = await axios.get<SearchResponse>(
      `https://financialmodelingprep.com/stable/search-symbol?query=${query}&apikey=${apiKey}`
    );
    return data;
  } catch (err) {
    if (axios.isAxiosError(err)) {
      console.log("error message: ", err.message);
      return err.message;
    } else {
      console.log("api error", err);
      return "An unexpected error has occured";
    }
  }
};

export const companyProfile = async (query: string) => {
  try {
    const data = await axios.get<CompanyProfile[]>(
      `https://financialmodelingprep.com/stable/profile?symbol=${query}&apikey=${apiKey}`
    );
    return data;
  } catch (err) {
    if (axios.isAxiosError(err)) {
      console.log("error message", err.message);
    } else {
      console.log("api error", err);
    }
  }
};

export const keyMetricsTTM = async (query: string) => {
  try {
    const data = await axios.get<CompanyKeyMetrics[]>(
      `https://financialmodelingprep.com/stable/key-metrics-ttm?symbol=${query}&apikey=${apiKey}`
    );
    return data;
  } catch (err) {
    if (axios.isAxiosError(err)) {
      console.log("error message", err.message);
    } else {
      console.log("api error", err);
    }
  }
};

export const incomeStatement = async (query: string) => {
  try {
    const data = await axios.get<CompanyIncomeStatement[]>(
      `https://financialmodelingprep.com/stable/income-statement?symbol=${query}&apikey=${apiKey}`
    );
    return data;
  } catch (err) {
    if (axios.isAxiosError(err)) {
      console.log("error message", err.message);
    } else {
      console.log("Api error", err);
    }
  }
};

export const balanceSheet = async (query: string) => {
  try {
    const data = await axios.get<CompanyBalanceSheet[]>(
      `https://financialmodelingprep.com/stable/balance-sheet-statement?symbol=${query}&apikey=${apiKey}`
    );
    return data;
  } catch (err) {
    if (axios.isAxiosError(err)) {
      console.log("error message", err.message);
    } else {
      console.log("api error", err);
    }
  }
};

export const cashFlowStatement = async (query: string) => {
  try {
    const data = await axios.get<CompanyCashFlow[]>(
      `https://financialmodelingprep.com/stable/cash-flow-statement?symbol=${query}&apikey=${apiKey}`
    );
    return data;
  } catch (err) {
    if (axios.isAxiosError(err)) {
      console.log("error message", err.message);
    } else {
      console.log("api error", err);
    }
  }
};

export const stockCompetition = async (query: string) => {
  try {
    const data = await axios.get<CompanyCompData>(
      `https://financialmodelingprep.com/stable/stock_peers?symbol=${query}&apikey=${apiKey}`
    );
    return data;
  } catch (err) {
    if (axios.isAxiosError(err)) {
      console.log("error message", err.message);
    } else {
      console.log("api error", err);
    }
  }
};

export const tenK = async (query: string) => {
  try {
    const data = await axios.get<tenKData[]>(
      `https://financialmodelingprep.com/stable/sec_filings/${query}?type=10-k&page=0&apikey=${apiKey}`
    );
    return data;
  } catch (err) {
    if (axios.isAxiosError(err)) {
      console.log("error message", err.message);
    } else {
      console.log("api error", err);
    }
  }
};
