import { getAddCedearData } from "@/loaders/loader-add-cedears";
import AddCedear from "../components/add-cedears/add-cedear";
import Dashboard from "../components/dashboard/dashboard";

import { createHashRouter } from "react-router-dom";

const routes = createHashRouter([
  {
    path: "/",
    element: <Dashboard />
  },
  {
    path: "/cedear",
    loader: getAddCedearData,
    element: <AddCedear />
  }
]);

export default routes;
