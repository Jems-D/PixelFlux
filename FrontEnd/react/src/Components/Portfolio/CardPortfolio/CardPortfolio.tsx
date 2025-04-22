import React, { SyntheticEvent } from "react";
import DeletePortfolio from "../DeletePorfolio/DeletePortfolio";
import { PortfolioGet } from "../../../Models/Portfolio";

interface Props {
  portfolios: PortfolioGet;
  onDeletePortfolio: (e: SyntheticEvent) => void;
}

const CardPortfolio = ({ portfolios, onDeletePortfolio }: Props) => {
  return (
    <div className="flex flex-col w-full p-8 space-y-4 text-center rounded-lg shadow-lg md:w-1/3">
      <p className="pt-6 text-xl font-bold">{portfolios.symbol}</p>
      <DeletePortfolio
        portfolio={portfolios.symbol}
        onDeletePortfolio={onDeletePortfolio}
      />
    </div>
  );
};

export default CardPortfolio;
