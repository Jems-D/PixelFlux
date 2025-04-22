import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { CompanyProfile } from "../../company";
import { companyProfile } from "../../api";
import Sidebar from "../../Components/Sidebar/Sidebar";
import CompanyDashboard from "../../Components/CompanyDashboard/CompanyDashboard";
import Tiles from "../../Components/Tiles/Tiles";
import Spinner from "../../Components/Spinner/Spinner";
import { formatLargeMonetaryNumber } from "../../Helpers/NumberFormatting";
type Props = {};

const CompanyPage = (props: Props) => {
  //gets the query from the link itself
  let { ticker } = useParams();
  const [companyData, setCompanyData] = useState<CompanyProfile>();

  useEffect(() => {
    const getCompanyProfile = async () => {
      const result = await companyProfile(ticker!);
      setCompanyData(result?.data[0]);
    };
    getCompanyProfile();
  }, []);

  return (
    <>
      {companyData ? (
        <div className="w-full relative flex ct-docs-disable-sidebar-content overflow-x-hidden">
          <Sidebar />
          <CompanyDashboard ticker={ticker!}>
            <Tiles title="Company Name" subTitle={companyData.companyName} />
            <Tiles title="Location" subTitle={companyData.address} />
            <Tiles title="CEO" subTitle={companyData.ceo} />
            <Tiles
              title="Market Cap"
              subTitle={formatLargeMonetaryNumber(companyData.marketCap)}
            />
            <p className="bg-white shadow rounded text-meduim text-gray-900 mt-3 mx-4 px-4">
              {companyData.description}
            </p>
            {/*<CompFinder ticker={`tsla`} />*/}
            {/*<TenKFinder ticker={companyData.symbol}/>*/}
          </CompanyDashboard>
        </div>
      ) : (
        <Spinner />
      )}
    </>
  );
};

export default CompanyPage;
