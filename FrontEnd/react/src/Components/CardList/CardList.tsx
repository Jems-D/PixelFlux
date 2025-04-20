import React, { JSX, SyntheticEvent } from "react";
import Card from "../Card/Card";
import { CompanySearch } from "../../company";

interface Props {
  searchResults: CompanySearch[];
  onPortfolioCreate: (e: SyntheticEvent) => void;
}

const CardList: React.FC<Props> = ({
  searchResults,
  onPortfolioCreate,
}: Props): JSX.Element => {
  const picture = "https://i.ibb.co/Fk5D5x1b/wind-of-eagle.png";
  let val: number = 20;

  return (
    <>
      {searchResults.length > 0 ? (
        searchResults.map((result, index) => {
          return (
            <Card
              id={result.symbol}
              key={index}
              searchResult={result}
              onPortfolioCreate={onPortfolioCreate}
            />
          );
        })
      ) : (
        <p className="mb-3  mt-3 text-xl font-semibold text-center md:text-xl">
          No results!
        </p>
      )}
    </>
  );
};

export default CardList;
