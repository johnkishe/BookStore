export const GENRES = [
  'Fiction',
  'SciFi',
  'Fantasy',
  'Mystery',
  'Biography',
  'NonFiction',
  'Children',
  'History',
] as const;

export type Genre = (typeof GENRES)[number];

export interface Book {
  id: string;
  title: string;
  author: string;
  isbn: string | null;
  publishedYear: number | null;
  coverUrl: string | null;
  openLibraryKey: string;
  description: string | null;
  genre: Genre;
  price: number;
  stock: number;
}

/** The fields the store owns and can change after a book is added. */
export interface StoreDetails {
  genre: Genre;
  price: number;
  stock: number;
  description: string | null;
}
