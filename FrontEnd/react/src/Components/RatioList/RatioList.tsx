import React from "react";
import { testIncomeStatementData } from "../Table/testData";

interface Props {
  config: any;
  data: any;
}

const RatioList = ({ config, data }: Props) => {
  const renderedRows = config.map((row: any, index: number) => {
    return (
      <li className="py-3 sm:py-4" key={index}>
        <div className="flex items-center space-x-4">
          <div className="flex-1 min-w-0">
            <p className="text-sm font-meduim text-gray-900 truncate">
              {row.label}
            </p>
            <p className="text-sm text-gray-500 truncate">
              {row.subTittle && row.subTittle}
            </p>
          </div>
          <div className="inline-flex items-center text-base font-semibold text-gray-900">
            {row.render(data)}
          </div>
        </div>
      </li>
    );
  });

  return (
    <div className="bg-white shadow rounded-lg ml-4 mt-4 mb-4 p-4 sm:p-6 h-full">
      <ul className="divide-y divided-gray-200">{renderedRows}</ul>
    </div>
  );
};

export default RatioList;
