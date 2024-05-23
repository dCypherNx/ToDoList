export class ToDo {
  id?: number;
  description?: string;
  date?: Date;
  status: number;

  constructor(
    description?: string,
    date?: Date,
    status: number = 0
  ) {
    this.description = description;
    this.date = date;
    this.status = status;
  }
}
