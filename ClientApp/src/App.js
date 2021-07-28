import React from "react";
import "./App.css";
import "../node_modules/bootstrap/dist/css/bootstrap.css";
import Calculator from "./components/Calculator";
import {
  BrowserRouter as Router,
  Route,
  Switch,
  withRouter
} from "react-router-dom";
import NotFound from "./components/NotFound";

function App(props) {
  return (
    <Router>
      <div className="App">
        <Switch>
           <Route exact path="/" component={Calculator} />

          <Route component={NotFound} />
        </Switch>
      </div>
    </Router>
  );
}

export default App;
