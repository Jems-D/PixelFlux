import React from "react";
import { Link } from "react-router-dom";

interface Props {
  ticker: string;
}

const CompFinderItem = ({ ticker }: Props) => {
  return (
    <>
      <Link
        reloadDocument
        to={`/company/${ticker}/company-profile`}
        type="button"
        className="inline-flex items-center p-4 rounded-sm"
      >
        ${ticker}
      </Link>
    </>
  );
};

export default CompFinderItem;
