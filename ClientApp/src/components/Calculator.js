import React, { useState, useEffect } from "react";
import axios from "axios";
import { Link } from "react-router-dom";

const Calculator = () => {
    const [cal, setCal] = useState({
        id: "",
        op1: "",
        op2: "",
        opp: "",
        result: ""
    });
    const { id, op1, op2, opp, result } = cal;

    const [calHistory, setHistoryCal] = useState([]);
    useEffect(() => {
        loadHistory();
    }, []);

    const loadHistory = async () => {
        const result = await axios.get("https://localhost:44320/api/Calculator/GetCalculationHistory");
        setHistoryCal(result.data.reverse());
    };

    const deleteRecord = async id => {
        await axios.delete(`https://localhost:44320/api/Calculator/DeleteHistoryById/${id}`);
        loadHistory();
    };
    
    const onInputChange = e => {
        setCal({ ...cal, [e.target.name]: e.target.value });
    };
    const clearState = () => {
        setCal({
            id: "", op1: "", op2: "", opp: "+", result: ""
        });
    }

    const onSubmit = async e => {
        e.preventDefault();
        if (cal.op1 == "" || cal.op2 == "") {
            return;
        }

        if (cal.id == "") {
            const obj = { op1: parseFloat(cal.op1), op2: parseFloat(cal.op2), opp: (cal.opp == "") ? "+" : cal.opp };
            const rslt = await axios.post("https://localhost:44320/api/Calculator/Calculate", obj);
            setCal({ ...cal, "result": rslt.data });
        }
        else {
            const obj = { id: cal.id, op1: parseFloat(cal.op1), op2: parseFloat(cal.op2), opp: (cal.opp == "") ? "+" : cal.opp, result: "" };
            const rslt1 =await axios.put(`https://localhost:44320/api/Calculator/UpdateHistory/`, obj);
            setCal({ ...cal, "result": rslt1.data });
            clearState();
        }
        loadHistory();
    };

    const editRecord = async id => {
        const result = await axios.get(`https://localhost:44320/api/Calculator/GetHistoryById/${id}`);
        setCal(result.data);
        console.log(result.data);
    };

    return (
        <div className="container">
            <div className="row">
                <div className="container">
                    <div className="w-75 mx-auto shadow p-5">
                        <h2 className="text-center mb-4">Calculator</h2>
                        <form onSubmit={e => onSubmit(e)}>
                            <div className="form-group">
                                <input
                                    type="number"
                                    className="form-control form-control-lg"
                                    placeholder="Enter 1st Value"
                                    name="op1"
                                    value={op1}
                                    onChange={e => onInputChange(e)}
                                />
                            </div>
                            <div className="form-group">
                                <select name="opp" value={opp} name="opp" className="form-control" onChange={e => onInputChange(e)}>
                                    <option value="+">+</option>
                                    <option value="-">-</option>
                                    <option value="*">*</option>
                                    <option value="/">/</option>
                                </select>
                            </div>
                            <div className="form-group">
                                <input
                                    type="number"
                                    className="form-control form-control-lg"
                                    placeholder="Enter 2nd Value"
                                    name="op2"
                                    value={op2}
                                    onChange={e => onInputChange(e)}
                                />
                            </div>
                            <div className="form-group">
                                <input
                                    disabled
                                    type="number"
                                    className="form-control form-control-lg"
                                    placeholder="Result"
                                    name="result"
                                    value={result}
                                />
                            </div>

                            <button className="btn btn-primary btn-block col-md-3">=</button>
                        </form>
                    </div>
                </div>
            </div>

            <div className="row">
                <div className="py-4 col-md-12">
                <h1>Calculator History</h1>
                <table className="table border shadow">
                    <thead className="thead-dark">
                        <tr>
                            <th scope="col">#</th>
                            <th scope="col">Op1</th>
                            <th scope="col">Opp</th>
                            <th scope="col">Op2</th>
                            <th scope="col">=</th>
                            <th scope="col">Result</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        {calHistory.map((calcu, index) => (
                            <tr>
                                <th scope="row">{index + 1}</th>
                                <td>{calcu.op1}</td>
                                <td>{calcu.opp}</td>
                                <td>{calcu.op2}</td>
                                <td>=</td>
                                <td>{calcu.result}</td>
                                <td>
                                    <Link className="btn btn-outline-primary mr-2" onClick={() => editRecord(calcu.id)}>Edit</Link>
                                    <Link className="btn btn-danger" onClick={() => deleteRecord(calcu.id)}>Delete</Link>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                </div>
            </div>
        </div>
    );
};

export default Calculator;
