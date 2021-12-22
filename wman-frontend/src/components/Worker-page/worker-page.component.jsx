import { Button } from "antd";
import { useState } from "react/cjs/react.development";
import WorkerThisWeek from "./worker-page-this-week-events/worker-page-this-week.component";
import WorkerToday from "./worker-page-today-events/worker-page-today";
import "./worker-page.styles.css"

const WorkerPage = () => {
    const [showToday, setShowToday] = useState(true);

    //handle changes
    const setToday = () => {
        setShowToday(true);
    }

    const setThisWeek = () => {
        setShowToday(false);
    }

    return (
        <div className="worker-page">
            <div className="select-intervall">
                <div>
                <Button onClick={setToday} >Today</Button>
                </div>
                <div>
                <Button onClick={setThisWeek} >This Week</Button>
                </div>
            </div>
            <div className="rendered-intervall">
                {showToday ? <WorkerToday /> : <WorkerThisWeek />}
            </div>
        </div>
    )
}
export default WorkerPage;