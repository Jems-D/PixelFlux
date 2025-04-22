import { useEffect, useState } from "react";
import { CompanyKeyMetrics } from "../../company";
import { useOutletContext } from "react-router";
import { keyMetricsTTM } from "../../api";
import RatioList from "../RatioList/RatioList";
import Spinner from "../Spinner/Spinner";
import {
  formatLargeMonetaryNumber,
  formatLargeNonMonetaryNumber,
  formatRatio,
} from "../../Helpers/NumberFormatting";
import StockComment from "../StockComment/StockComment";

interface Props {}

const tableConfig = [
  {
    label: "Market Cap",
    render: (company: CompanyKeyMetrics) =>
      formatLargeNonMonetaryNumber(company.marketCap),
    subTitle: "Total value of all a company's shares of stock",
  },
  {
    label: "Current Ratio",
    render: (company: CompanyKeyMetrics) =>
      formatRatio(company.currentRatioTTM),
    subTitle:
      "Measures the companies ability to pay short term debt obligations",
  },
  {
    label: "Return On Equity",
    render: (company: CompanyKeyMetrics) =>
      formatRatio(company.returnOnEquityTTM),
    subTitle:
      "Return on equity is the measure of a company's net income divided by its shareholder's equity",
  },
  {
    label: "Return On Assets",
    render: (company: CompanyKeyMetrics) =>
      formatRatio(company.returnOnTangibleAssetsTTM),
    subTitle:
      "Return on assets is the measure of how effective a company is using its assets",
  },
  {
    label: "Free Cashflow Per Share",
    render: (company: CompanyKeyMetrics) =>
      formatRatio(company.freeCashFlowYieldTTM),
    subTitle:
      "Return on assets is the measure of how effective a company is using its assets",
  },
  {
    label: "Book Value Per Share TTM",
    render: (company: CompanyKeyMetrics) =>
      formatLargeMonetaryNumber(company.bookValuePerShareTTM),
    subTitle:
      "Book value per share indicates a firm's net asset value (total assets - total liabilities) on per share basis",
  },
  {
    label: "Divdend Yield TTM",
    render: (company: CompanyKeyMetrics) =>
      formatRatio(company.dividendYieldTTM),
    subTitle: "Shows how much a company pays each year relative to stock price",
  },
  {
    label: "Capex Per Share TTM",
    render: (company: CompanyKeyMetrics) =>
      formatRatio(company.capexToOperatingCashFlowTTM),
    subTitle:
      "Capex is used by a company to aquire, upgrade, and maintain physical assets",
  },
  {
    label: "Graham Number",
    render: (company: CompanyKeyMetrics) =>
      formatLargeMonetaryNumber(company.grahamNumberTTM),
    subTitle:
      "This is the upperbouind of the price range that a defensive investor should pay for a stock",
  },
  {
    label: "PE Ratio",
    render: (company: CompanyKeyMetrics) =>
      formatRatio(company.currentRatioTTM),
    subTitle:
      "This is the upperbouind of the price range that a defensive investor should pay for a stock",
  },
];

const CompanyProfile = (props: Props) => {
  const ticker = useOutletContext<string>();
  const [companyKeyMetrics, setCompanyKeyMetrics] =
    useState<CompanyKeyMetrics>();

  useEffect(() => {
    const getKeyMetrics = async () => {
      const value = await keyMetricsTTM(ticker);
      setCompanyKeyMetrics(value?.data[0]);
    };
    getKeyMetrics();
  }, []);

  return (
    <>
      {companyKeyMetrics ? (
        <>
          <RatioList config={tableConfig} data={companyKeyMetrics} />
          <StockComment symbol={ticker} />
        </>
      ) : (
        <Spinner />
      )}
    </>
  );
};

export default CompanyProfile;
