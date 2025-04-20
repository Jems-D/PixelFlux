import { CompanyBalanceSheet } from "../../company";
import { useOutletContext } from "react-router";
import { balanceSheet } from "../../api";
import Table from "../Table/Table";
import Spinner from "../Spinner/Spinner";
import { formatLargeMonetaryNumber } from "../../Helpers/NumberFormatting";
import { useEffect, useState } from "react";

type Props = {};

const config = [
  {
    label: "Date",
    render: (company: CompanyBalanceSheet) => company.date,
  },
  {
    label: "Total Assets",
    render: (company: CompanyBalanceSheet) =>
      formatLargeMonetaryNumber(company.totalAssets),
  },
  {
    label: "Total Cash",
    render: (company: CompanyBalanceSheet) =>
      formatLargeMonetaryNumber(company.cashAndCashEquivalents),
  },
  {
    label: "Intagible Asset",
    render: (company: CompanyBalanceSheet) =>
      formatLargeMonetaryNumber(company.intangibleAssets),
  },
  {
    label: "Long Term Debt",
    render: (company: CompanyBalanceSheet) =>
      formatLargeMonetaryNumber(company.longTermDebt),
  },
  {
    label: "Total Debt",
    render: (company: CompanyBalanceSheet) =>
      formatLargeMonetaryNumber(company.totalDebt),
  },
  {
    label: "Total Liabilities",
    render: (company: CompanyBalanceSheet) =>
      formatLargeMonetaryNumber(company.totalLiabilities),
  },
  {
    label: "Total Current Liabilities",
    render: (company: CompanyBalanceSheet) =>
      formatLargeMonetaryNumber(company.totalCurrentLiabilities),
  },
];

const BalanceSheet = (props: Props) => {
  const ticker = useOutletContext<string>();
  const [balanceSheetData, setBalanceSheet] = useState<CompanyBalanceSheet[]>(
    []
  );
  useEffect(() => {
    const getBalanceSheet = async () => {
      const result = await balanceSheet(ticker);
      setBalanceSheet(result!.data);
    };
    getBalanceSheet();
  }, []);
  return (
    <>
      {balanceSheetData ? (
        <Table configs={config} data={balanceSheetData} />
      ) : (
        <Spinner />
      )}
    </>
  );
};

export default BalanceSheet;
