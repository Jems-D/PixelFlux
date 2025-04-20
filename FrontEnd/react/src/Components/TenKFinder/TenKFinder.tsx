import React, { useEffect, useState } from "react";
import { tenKData } from "../../company";
import { tenK } from "../../api";
import Spinner from "../Spinner/Spinner";
import { data } from "react-router";
import TenKFinderItem from "./TenKFinderItem";
interface Props {
  ticker: string;
}

const TenKFinder = ({ ticker }: Props) => {
  const [tenKData, setTenK] = useState<tenKData[]>();
  useEffect(() => {
    const fetchTenK = async () => {
      const data = await tenK(ticker!);
      setTenK(data!.data);
    };
    fetchTenK();
  }, []);
  return (
    <div className="inline-flex rounded-md shadow-sm m-4">
      {tenKData ? (
        tenKData.slice(0, 5).map((data) => {
          return <TenKFinderItem tenK={data} />;
        })
      ) : (
        <Spinner />
      )}
    </div>
  );
};

export default TenKFinder;
