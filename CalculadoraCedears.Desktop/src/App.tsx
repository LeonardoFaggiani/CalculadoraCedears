import "./App.css";
import { RouterProvider } from "react-router-dom";
import router from "./router/router";
import { DataProvider } from "./context/data-context";
function App() {
  return (
    <DataProvider>
      <div className="min-h-screen">
        <main className="flex-1 mx-auto">
          <RouterProvider router={router} />
        </main>
      </div>
    </DataProvider>
  );
}

export default App;
