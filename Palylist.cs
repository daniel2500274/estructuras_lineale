using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;

namespace Playlist_circular_con_prev_next
{
    internal class Playlist : IEnumerable<Song>
    {
        private class Node
        {
            public Song Data { get; set; }
            public Node? Next { get; set; }
            public Node? Prev { get; set; }

            public Node(Song song)
            {
                Data = song;
            }
        }

        private Node? _head;
        private Node? _current;
        public int Count { get; private set; }

        public Playlist()
        {
            _head = null;
            _current = null;
            Count = 0;
        }

        // Add a song to the end of the playlist
        public void AddLast(Song song)
        {
            if (song == null)
                throw new ArgumentNullException(nameof(song));

            var newNode = new Node(song);

            if (_head == null)
            {
                // First node - circular reference to itself
                _head = newNode;
                _head.Next = _head;
                _head.Prev = _head;
                _current = _head;
            }
            else
            {
                // Insert at the end (before head)
                var tail = _head.Prev!;

                tail.Next = newNode;
                newNode.Prev = tail;
                newNode.Next = _head;
                _head.Prev = newNode;
            }

            Count++;
        }

        // Remove a song by its ID
        public bool RemoveById(Guid id)
        {
            if (_head == null)
                return false;

            Node? nodeToRemove = null;
            var current = _head;

            // Search for the node with matching ID
            do
            {
                if (current.Data.Id == id)
                {
                    nodeToRemove = current;
                    break;
                }
                current = current.Next!;
            } while (current != _head);

            if (nodeToRemove == null)
                return false;

            // Only one node in the list
            if (Count == 1)
            {
                _head = null;
                _current = null;
            }
            else
            {
                // Update references
                nodeToRemove.Prev!.Next = nodeToRemove.Next;
                nodeToRemove.Next!.Prev = nodeToRemove.Prev;

                // Update head if we're removing it
                if (nodeToRemove == _head)
                    _head = nodeToRemove.Next;

                // Update current if we're removing it
                if (nodeToRemove == _current)
                    _current = nodeToRemove.Next;
            }

            Count--;
            return true;
        }

        // Move to the next song circularly
        public Song Next()
        {
            if (_current == null)
                throw new InvalidOperationException("Playlist is empty");

            _current = _current.Next!;
            return _current.Data;
        }

        // Move to the previous song circularly
        public Song Prev()
        {
            if (_current == null)
                throw new InvalidOperationException("Playlist is empty");

            _current = _current.Prev!;
            return _current.Data;
        }

        // Get the current song without moving the cursor
        public Song Current()
        {
            if (_current == null)
                throw new InvalidOperationException("Playlist is empty");

            return _current.Data;
        }

        // Shuffle the playlist using Fisher-Yates algorithm
        public void Shuffle(int seed)
        {
            if (Count <= 1)
                return;

            // Convert to array for shuffling
            var songs = new Song[Count];
            int index = 0;
            var current = _head;

            do
            {
                songs[index++] = current!.Data;
                current = current.Next;
            } while (current != _head);

            // Fisher-Yates shuffle
            var random = new Random(seed);
            for (int i = Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (songs[i], songs[j]) = (songs[j], songs[i]);
            }

            // Rebuild the circular linked list with shuffled order
            _head = null;
            _current = null;
            Count = 0;

            foreach (var song in songs)
            {
                AddLast(song);
            }
        }

        // Export playlist titles to JSON
        public string ExportTitlesJson()
        {
            if (_head == null)
                return "[]";

            var titles = new List<string>();
            var current = _head;

            do
            {
                titles.Add(current!.Data.Title);
                current = current.Next;
            } while (current != _head);

            return JsonSerializer.Serialize(titles, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
        }

        // IEnumerable implementation for foreach support
        public IEnumerator<Song> GetEnumerator()
        {
            if (_head == null)
                yield break;

            var current = _head;
            do
            {
                yield return current.Data;
                current = current.Next!;
            } while (current != _head);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
