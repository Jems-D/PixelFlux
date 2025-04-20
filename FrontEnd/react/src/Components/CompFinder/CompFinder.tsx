import React, { useEffect, useState } from "react";
import { CompanyCompData } from "../../company";
import { stockCompetition } from "../../api";
import Spinner from "../Spinner/Spinner";
import CompFinderItem from "./CompFinderItem";

interface Props {
  ticker: string;
}

const CompFinder = ({ ticker }: Props) => {
  const [companyComp, setCompanyComp] = useState<CompanyCompData>();
  useEffect(() => {
    const fetchCompanyComp = async () => {
      const companyCompData = await stockCompetition(ticker!);
      setCompanyComp(companyCompData!.data);
    };
    fetchCompanyComp();
  }, []);
  return (
    <div className="inline-flex rounded-md shadow-sm m-4">
      {companyComp?.peersList.map((data) => {
        return <CompFinderItem ticker={data} />;
      })}
    </div>
  );
};

export default CompFinder;
