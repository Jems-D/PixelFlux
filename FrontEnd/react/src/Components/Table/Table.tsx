import React from "react";
import { testIncomeStatementData } from "./testData";
import { CompanyIncomeStatement } from "../../company";

interface Props {
  configs: any;
  data: any;
}
const Table = ({ configs, data }: Props) => {
  const renderedRows = data.map((data: any, index: number) => {
    return (
      <tr key={`index-${index}`}>
        {configs.map((val: any, dataIndex: number) => {
          return (
            <td
              key={`cell-${dataIndex}`}
              className="p-4 whitespace-nowrap text-sm font-normal text-gray-900"
            >
              {val.render(data)}
            </td>
          );
        })}
      </tr>
    );
  });

  const renderedHeaders = configs.map((data: any, index: number) => {
    return (
      <th
        className="p-4 text-left text-xs font-medium text-fray-500 uppercase tracking-wider"
        key={index}
      >
        {data.label}
      </th>
    );
  });

  return (
    <div className="bg-white shadow rounded-lg p-4 sm:p-6 xl:p-8 mx-4 mt-3">
      <table>
        <thead className="min-w-full divide-y divide-gray-200 m-5">
          {renderedHeaders}
          <tr></tr>
        </thead>
        <tbody>{renderedRows}</tbody>
      </table>
    </div>
  );
};

export default Table;
