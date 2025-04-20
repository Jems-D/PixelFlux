import React, { SyntheticEvent } from "react";
import CardPortfolio from "../CardPortfolio/CardPortfolio";

interface Props {
  portfolios: string[];
  onDeletePortfolio: (e: SyntheticEvent) => void;
}

const PortfolioList = ({ portfolios, onDeletePortfolio }: Props) => {
  return (
    <section id="portfolio">
      <h2 className="mb-3 mt-3 text-3xl font-semibold text-center md:text-4xl">
        My Portfolio
      </h2>
      <div className="relative flex flex-col items-center max-w-5xl mx-auto space-y-10 px-10 mb-5 md:px-6 md:space-y-0 md:space-x-7 md:flex-row">
        <>
          {portfolios.length > 0 ? (
            portfolios.map((portfolioValue) => {
              return (
                <CardPortfolio
                  portfolios={portfolioValue}
                  onDeletePortfolio={onDeletePortfolio}
                />
              );
            })
          ) : (
            <h3 className="mb-3 mt-3 text-xl font-semibold text-center md:text-xl">
              Your portfolio is empty.
            </h3>
          )}
        </>
      </div>
    </section>
  );
};

export default PortfolioList;
