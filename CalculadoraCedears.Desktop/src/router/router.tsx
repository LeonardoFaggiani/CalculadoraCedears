import Login from "../components/login/login";
import AddCedear from "../components/add-cedears/add-cedear";
import Dashboard from "../components/dashboard/dashboard";
import CheckUpdates from "@/components/check-updates/check-updates";

import { createHashRouter } from "react-router-dom";

const routes = createHashRouter([
  {
    path: "/checkupdates",
    element: <CheckUpdates />
  },
  {
    path: "/",
    element: <Login />
  },
  {
    path: "/home",
    element: <Dashboard />
  },
  {
    path: "/cedear",
    element: <AddCedear />
  }
]);

export default routes;
