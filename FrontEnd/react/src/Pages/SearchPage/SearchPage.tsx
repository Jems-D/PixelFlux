import React, {
  ChangeEvent,
  JSX,
  lazy,
  Suspense,
  SyntheticEvent,
  useState,
} from "react";
import Search from "../../Components/Search/Search";
import PortfolioList from "../../Components/Portfolio/List Portfolio/PortfolioList";
const CardList = lazy(() => import("../../Components/CardList/CardList"));
import { searchCompanies } from "../../api";
import { CompanySearch } from "../../company";

interface Props {}

const SearchPage = (props: Props) => {
  const [search, setSearch] = useState<string>("");
  const [searchResult, setSearchResult] = useState<CompanySearch[]>([]);
  const [serverError, setServerError] = useState<string>("");
  const [portfolioArray, setPortfolio] = useState<string[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    setSearch(e.target.value);
  };

  const onSearchSubmit = async (e: SyntheticEvent) => {
    e.preventDefault();
    setIsLoading(true);
    const result = await searchCompanies(search);
    if (typeof result === "string") {
      setServerError(result);
      console.log(serverError);
    } else if (Array.isArray(result.data)) {
      setSearchResult(result.data);
      setIsLoading(false);
      console.log(searchResult);
    }
  };

  const onPortfolioCreate = (e: any): void => {
    e.preventDefault();
    const exist = portfolioArray.find((value) => value === e.target[0].value);
    if (exist) return;
    const updatedPortfolio = [...portfolioArray, e.target[0].value];
    setPortfolio(updatedPortfolio);
  };

  const onPortfolioDelete = (e: any): void => {
    e.preventDefault();
    const removed = portfolioArray.filter((value) => {
      return value !== e.target[0].value;
    });
    setPortfolio(removed);
  };
  const Loading = (): JSX.Element => {
    return <h2>🌀 Loading..</h2>;
  };

  return (
    <>
      <Search
        search={search}
        handleInputChange={handleInputChange}
        onSearchSubmit={onSearchSubmit}
      />
      <PortfolioList
        portfolios={portfolioArray}
        onDeletePortfolio={onPortfolioDelete}
      />
      {isLoading ? (
        <Loading />
      ) : (
        <Suspense fallback={<Loading />}>
          <CardList
            searchResults={searchResult}
            onPortfolioCreate={onPortfolioCreate}
          />
        </Suspense>
      )}
      {serverError && <h1>Network Error</h1>}
    </>
  );
};

export default SearchPage;
