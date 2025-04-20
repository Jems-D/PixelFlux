import { useEffect, useState } from "react";
import { CompanyCashFlow } from "../../company";
import { useOutletContext } from "react-router";
import { cashFlowStatement } from "../../api";
import Table from "../Table/Table";
import Spinner from "../Spinner/Spinner";
import { formatLargeMonetaryNumber } from "../../Helpers/NumberFormatting";

type Props = {};

const config = [
  {
    label: "Date",
    render: (company: CompanyCashFlow) => company.date,
  },
  {
    label: "Operating Cashflow",
    render: (company: CompanyCashFlow) =>
      formatLargeMonetaryNumber(company.operatingCashFlow),
  },
  {
    label: "Investing Cashflow",
    render: (company: CompanyCashFlow) =>
      formatLargeMonetaryNumber(company.netCashProvidedByInvestingActivities),
  },
  {
    label: "Financing Cashflow",
    render: (company: CompanyCashFlow) =>
      company.netCashProvidedByFinancingActivities,
  },
  {
    label: "Cash At End of Period",
    render: (company: CompanyCashFlow) =>
      formatLargeMonetaryNumber(company.cashAtEndOfPeriod),
  },
  {
    label: "CapEX",
    render: (company: CompanyCashFlow) =>
      formatLargeMonetaryNumber(company.capitalExpenditure),
  },
  {
    label: "Issuance Of Stock",
    render: (company: CompanyCashFlow) =>
      formatLargeMonetaryNumber(company.commonStockIssuance),
  },
  {
    label: "Free Cash Flow",
    render: (company: CompanyCashFlow) =>
      formatLargeMonetaryNumber(company.freeCashFlow),
  },
];

const CashFlowStatement = (props: Props) => {
  const ticker = useOutletContext<string>();
  const [cashFlowStatementData, setCashFlowStatementData] = useState<
    CompanyCashFlow[]
  >([]);
  useEffect(() => {
    const getCashFlowStatementData = async () => {
      const cashFlow = await cashFlowStatement(ticker!);
      setCashFlowStatementData(cashFlow!.data);
    };
    getCashFlowStatementData();
  }, []);

  return (
    <>
      {cashFlowStatementData ? (
        <Table configs={config} data={cashFlowStatementData} />
      ) : (
        <Spinner />
      )}
    </>
  );
};

export default CashFlowStatement;
