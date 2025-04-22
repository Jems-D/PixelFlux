export type PortfolioGet = {
  id: number;
  symbol: string;
  companyName: string;
  purchase: number;
  lastDiv: number;
  industry: string;
  marketCap: number;
  comment: any;
};

export type PorfolioPost = {
  symbol: string;
};
