import React from "react";
import { tenKData } from "../../company";
import { Link } from "react-router-dom";

interface Props {
  tenK: tenKData;
}

const TenKFinderItem = ({ tenK }: Props) => {
  const fillingDate = new Date(tenK.fillingDate).getFullYear();

  return (
    <Link
      reloadDocument
      to={tenK.finalLink}
      type="button"
      aria-label="button for chosen company's sec filling"
      className="inline-flex items-center p-4 text-md text-white bg-green rouded-md"
    >
      10K - {tenK.symbol} - {fillingDate}
    </Link>
  );
};

export default TenKFinderItem;
