import "./App.css";
import { HashRouter, Routes, Route } from "react-router-dom";
import Dashboard from "./components/dashboard/dashboard";
import AddCedear from "./components/add-cedears/add-cedear";

function App() {
  return (
    <div className="flex flex-col min-h-screen">
      <main className="flex-1 container mx-auto p-8">
        <HashRouter>
          <Routes>
            <Route path="/" element={<Dashboard />} />
            <Route path="/cedear" element={<AddCedear />} />
          </Routes>
        </HashRouter>
      </main>
    </div>
  );
}

export default App;
